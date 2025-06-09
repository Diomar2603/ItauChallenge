using ItauChallenge.Application.Interfaces;
using ItauChallenge.Application.Services;
using ItauChallenge.Core.Interfaces;
using ItauChallenge.Infrastructure.Data;
using ItauChallenge.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfiguration configuration = builder.Configuration;
string connectionString = configuration.GetConnectionString("DefaultBdConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'DefaultBdConnection' não foi encontrada.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddScoped<ICotacoesRepository, CotacoesRepository>();
builder.Services.AddScoped<IOperacoesRepository, OperacoesRepository>();
builder.Services.AddScoped<IPosicoesRepository, PosicoesRepository>();
builder.Services.AddScoped<IAtivosRepository, AtivosRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<ICotacaoService, CotacaoService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<IOperacoesRepository, OperacoesRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
