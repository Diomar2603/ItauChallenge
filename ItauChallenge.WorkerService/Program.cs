using ItauChallenge.Core.Interfaces;
using ItauChallenge.Infrastructure.Data;
using ItauChallenge.Infrastructure.Data.Repositories;
using ItauChallenge.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        string connectionString = configuration.GetConnectionString("DefaultBdConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("A string de conexão 'DefaultBdConnection' não foi encontrada.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        services.Configure<KafkaConnectionSettings>(
            hostContext.Configuration.GetSection("KafkaConnectionSettings")
        );

        services.AddHostedService<CotacoesConsumerWorker>();


        services.AddScoped<ICotacoesRepository, CotacoesRepository>();
        services.AddScoped<IAtivosRepository, AtivosRepository>();
    })
    .Build();

await host.RunAsync();