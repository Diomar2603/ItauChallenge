using ItauChallenge.Core.Interfaces;
using ItauChallenge.Infrastructure.Data;
using ItauChallenge.Infrastructure.Data.Repositories;
using ItauChallenge.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        string connectionString = configuration.GetConnectionString("DefaultBdConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        services.Configure<KafkaConnectionSettings>(
             hostContext.Configuration.GetSection("KafkaConnectionSettings")
        );

        // Atualizamos o nome aqui para registrar o worker correto
        services.AddHostedService<CotacoesConsumerWorker>();
        services.AddDbContext<AppDbContext>();
        services.AddSingleton<ICotacoesRepository, CotacoesRepository>();
    })
    .Build();

await host.RunAsync();