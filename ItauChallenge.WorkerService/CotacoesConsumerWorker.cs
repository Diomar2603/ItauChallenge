using Confluent.Kafka;
using ItauChallenge.Application.DTO;
using ItauChallenge.Core.Entities;
using ItauChallenge.Core.Interfaces;
using ItauChallenge.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Wrap;
using System.Text.Json;
using System.Threading;

public class CotacoesConsumerWorker : BackgroundService
{
    private readonly ILogger<CotacoesConsumerWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _bootstrapServers;
    private readonly string _groupId;

    private readonly AsyncPolicyWrap _resiliencePolicy;

    public CotacoesConsumerWorker(
        ILogger<CotacoesConsumerWorker> logger,
        IOptions<KafkaConnectionSettings> kafkaSettings,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;

        _bootstrapServers = kafkaSettings.Value.BootstrapServers;
        _groupId = kafkaSettings.Value.GroupId;

        if (string.IsNullOrEmpty(_bootstrapServers) || string.IsNullOrEmpty(_groupId))
        {
            throw new InvalidOperationException("As configurações do Kafka não foram encontradas.");
        }

        var fallbackPolicy = Policy
            .Handle<Exception>()
            .FallbackAsync(
                fallbackAction: (cancellationToken) =>
                {
                    _logger.LogCritical("PROCESSAMENTO FALHOU PERMANENTEMENTE. Fallback acionado..");
                    return Task.CompletedTask;
                },
                onFallbackAsync: (exception) =>
                {
                    _logger.LogError(exception, "Ação de fallback executada devido a uma falha persistente.");
                    return Task.CompletedTask;
                }
            );

        var circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (exception, timespan) => _logger.LogCritical(exception, $"CIRCUITO ABERTO por {timespan.TotalSeconds} segundos."),
                onReset: () => _logger.LogInformation("CIRCUITO FECHADO. O serviço parece normal."),
                onHalfOpen: () => _logger.LogWarning("CIRCUITO EM MODO HALF-OPEN.")
            );

        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(exception, $"Falha ao processar. Tentativa {retryCount}. Tentando novamente em {timespan.TotalSeconds}s.");
                });

        _resiliencePolicy = fallbackPolicy.WrapAsync(circuitBreakerPolicy.WrapAsync(retryPolicy));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("cotacoes-topic");
        _logger.LogInformation("Worker iniciado...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);
                var cotacaoJson = consumeResult.Message.Value;
                _logger.LogInformation($"Mensagem recebida: {cotacaoJson}");

                await _resiliencePolicy.ExecuteAsync(
                    async token => await ProcessarMensagemComEscopo(cotacaoJson, token),
                    stoppingToken
                );
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operação cancelada. Encerrando o worker.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro inesperado no loop principal do worker.");
            }
        }
    }

    private async Task ProcessarMensagemComEscopo(string cotacaoJson, CancellationToken stoppingToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var cotacoesRepository = scope.ServiceProvider.GetRequiredService<ICotacoesRepository>();
            var ativosRepository = scope.ServiceProvider.GetRequiredService<IAtivosRepository>();
            var cotacaoDto = JsonSerializer.Deserialize<CotacaoEntradaDto>(cotacaoJson);

            if (cotacaoDto == null)
            {
                _logger.LogWarning("Cotação nula recebida e ignorada.");
                return;
            }

            var ativo = await ativosRepository.GetByCodigoAsync(cotacaoDto.CodigoDoAtivo);

            if (ativo == null)
            {
                _logger.LogWarning($"Ativo referenciado na cotação não existe no sistema, logo a cotação não será registrada.");
                return;
            }

            var jaExiste = await cotacoesRepository.ExisteAsync(ativo.Id, cotacaoDto.DataHora);
            if (jaExiste)
            {
                _logger.LogWarning($"Cotação duplicada recebida e ignorada para o ativo {cotacaoDto.CodigoDoAtivo}.");
                return;
            }

            var cotacao = new Cotacao(ativo.Id, cotacaoDto.PrecoUnitario, cotacaoDto.DataHora);

            await cotacoesRepository.AdicionarAsync(cotacao);
            _logger.LogInformation($"Cotação para o ativo {cotacaoDto.CodigoDoAtivo} salva com sucesso.");
        }
    }
}