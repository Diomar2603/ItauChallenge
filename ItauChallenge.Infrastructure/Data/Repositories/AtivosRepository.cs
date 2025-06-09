using ItauChallenge.Core.Entities;
using ItauChallenge.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Infrastructure.Data.Repositories
{
    public class AtivosRepository : IAtivosRepository
    {
        private readonly AppDbContext _context;

        public AtivosRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ativo> GetByCodigoAsync(string codigoDoAtivo)
        {
            return await _context.Ativos.FirstAsync(a => a.Codigo == codigoDoAtivo);
        }

        public async Task<Ativo> GetByIdAsync(int ativoId)
        {
            return await _context.Ativos.FirstAsync(a => a.Id == ativoId);
        }
    }
}
