using ItauChallenge.Core.Entities;
using ItauChallenge.Core.Enums;
using ItauChallenge.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Infrastructure.Data.Repositories
{
    public class OperacoesRepository : IOperacoesRepository
    {
        private readonly AppDbContext _context;

        public OperacoesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Operacao>> GetByIdUsuarioAsync(int userId)
        {
            return await _context.Operacoes
                                 .Where(op => op.UsuarioId == userId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Operacao>> GetComprasPorUsuarioEAtivoAsync(int usuarioId, int ativoId)
        {
            return await _context.Operacoes
            .Where(op => op.UsuarioId == usuarioId)
            .Where(op => op.AtivoId == ativoId)
            .Where(op => op.Tipo == TipoOperacao.Compra)
            .ToListAsync();
        }
    }
}
