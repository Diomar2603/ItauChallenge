using ItauChallenge.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Operacao> Operacoes { get; set; }
        public DbSet<Posicao> Posicoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; } 
        public DbSet<Ativo> Ativos { get; set; } 
        public DbSet<Cotacao> Cotacoes { get; set; } 
    }
}
