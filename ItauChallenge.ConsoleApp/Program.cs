// Arquivo: ItaúChallenge.ConsoleApp/Program.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ItauChallenge.Core.Interfaces;
using ItauChallenge.Infrastructure.Data;
using ItauChallenge.Infrastructure.Data.Repositories;
using ItauChallenge.Application;
using System;
using ItauChallenge.Infrastructure.Data;
using ItauChallenge.Application.Services;
using Microsoft.Extensions.Hosting;
using ItauChallenge.Application.Interfaces;

public class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        // Local para configurar acesso ao MySQL
        string connectionString = "Server=localhost;Database=itau_challenge_db;User=root;Password=Lea@2023!;";

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySQL(connectionString)
        );

        services.AddScoped<IOperacoesRepository, OperacoesRepository>();
        services.AddScoped<IPosicoesRepository, PosicoesRepository>();
        services.AddScoped<IAtivosRepository, AtivosRepository>();
        services.AddScoped<IPortfolioService, PortfolioService>();

        var serviceProvider = services.BuildServiceProvider();

        var portfolioService = serviceProvider.GetRequiredService<IPortfolioService>();

        Console.WriteLine("Iniciando cálculo do portfólio...");
        int sampleUserId = 1;

        var summary = await portfolioService.GetPortifolioUsuarioAsync(sampleUserId);

        Console.WriteLine($"\n--- Resumo do Portfólio do Cliente: {summary.ClienteId} ---");
        Console.WriteLine($"Lucro/Prejuízo Global: {summary.LucroPrejuizoGlobal:C}");
        Console.WriteLine($"Total de Corretagem Paga: {summary.TotalCorretagemPaga:C}");
        Console.WriteLine("\nPosições por Ativo:");
        foreach (var pos in summary.Posicoes)
        {
            Console.WriteLine($"  - Ativo ID {pos.CodigoAtivo}:");
            Console.WriteLine($"    Quantidade Atual: {pos.QuantidadeAtual}");
            Console.WriteLine($"    Preço Médio: {pos.PrecoMedio:C}");
            Console.WriteLine($"    Total Investido (Compras): {pos.ValorTotalInvestido:C}");
        }
    }
}