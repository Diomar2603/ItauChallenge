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
    public class PosicoesRepository : IPosicoesRepository
    {
        private readonly AppDbContext _context;

        public PosicoesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Posicao>> GetAllAsync()
        {
            return await _context.Posicoes.ToListAsync();
        }

        public async Task<IEnumerable<Posicao>> GetByIdUsuarioAsync(int userId)
        {
            return await _context.Posicoes
                                 .Where(p => p.UsuarioId == userId)
                                 .ToListAsync();
        }
    }
}
