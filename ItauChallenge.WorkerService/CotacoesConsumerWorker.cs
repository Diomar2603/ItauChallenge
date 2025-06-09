using Confluent.Kafka;
using ItauChallenge.Core.Entities;
using ItauChallenge.Core.Interfaces;
using ItauChallenge.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Text.Json;
using System.Threading;
// using ItauChallenge.Core.Entities;
// using ItauChallenge.Core.Interfaces;
// using System.Text.Json;

public class CotacoesConsumerWorker : BackgroundService
{
    private readonly ILogger<CotacoesConsumerWorker> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly ICotacoesRepository _cotacoesRepository;
    private readonly IConfiguration _configuration;
    private readonly string _bootstrapServers;
    private readonly string _groupId;

    public CotacoesConsumerWorker(ILogger<CotacoesConsumerWorker> logger, ICotacoesRepository cotacoesRepository,
                                  IOptions<KafkaConnectionSettings> kafkaSettings)
    {
        _logger = logger;
        _cotacoesRepository = cotacoesRepository;

        _bootstrapServers = kafkaSettings.Value.BootstrapServers;
        _groupId = kafkaSettings.Value.GroupId;

        if (string.IsNullOrEmpty(_bootstrapServers) || string.IsNullOrEmpty(_groupId))
        {
            throw new InvalidOperationException("As configurações do Kafka (BootstrapServers, GroupId) não foram encontradas.");
        }

        _retryPolicy = Policy
            .Handle<Exception>() 
            .WaitAndRetryAsync(3, 
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timespan, retryCount, context) =>
                {
                    _logger.LogWarning($"Erro ao processar mensagem. Tentativa {retryCount}. Tentando novamente em {timespan.TotalSeconds}s. Erro: {exception.Message}");
                });
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

        _logger.LogInformation($"Worker iniciado. Escutando o tópico 'cotacoes-topic' no Kafka.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);
                var cotacaoJson = consumeResult.Message.Value;
                _logger.LogInformation($"Nova mensagem recebida: {cotacaoJson}");

                await _retryPolicy.ExecuteAsync(async () =>
                {
                    var cotacao = JsonSerializer.Deserialize<Cotacao>(cotacaoJson);
                    var jaExiste = false;

                    if (cotacao != null)
                    {
                        _logger.LogWarning($"Cotação sem informações recebida e ignorada.");
                        return;
                    }
                    else
                    {
                        jaExiste = await _cotacoesRepository.ExisteAssync(cotacao.AtivoId, cotacao.DataHora);
                    }
                    
                     if (jaExiste)
                     {
                         _logger.LogWarning($"Cotação duplicada recebida e ignorada para o ativo {cotacao.AtivoId}.");
                         return;
                     }

                    _cotacoesRepository.AdicionarAsync(cotacao);
                    _logger.LogInformation($"Cotação para o ativo {cotacao.AtivoId} salva com sucesso.");
                });
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operação cancelada. Encerrando o worker.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro inesperado ao consumir a mensagem do Kafka.");
            }
        }
    }
}