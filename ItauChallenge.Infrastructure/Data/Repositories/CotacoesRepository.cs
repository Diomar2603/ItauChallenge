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
    public class CotacoesRepository : ICotacoesRepository
    {
        private readonly AppDbContext _context;

        public CotacoesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Cotacao> AdicionarAsync(Cotacao cotacao)
        {
            var novaCotacao = await _context.Cotacoes.AddAsync(cotacao);
            await _context.SaveChangesAsync();

            return await Task.FromResult(novaCotacao.Entity);

        }

        public Task<bool> ExisteAsync(int ativoId, DateTime dataHora)
        {
            var cotacaoDuplicada = _context.Cotacoes.
                                            Where(c => c.AtivoId == ativoId & c.DataHora.Equals(dataHora))
                                            .FirstOrDefault();

            var exists = true;

            if (cotacaoDuplicada == null) 
                exists = false;

            return Task.FromResult(exists);
        }

        public async Task<Cotacao> GetUltimaPorAtivoIdAsync(int ativoId)
        {
            return await _context.Cotacoes
                         .Where(c => c.AtivoId == ativoId)
                         .OrderByDescending(c => c.DataHora) 
                         .FirstOrDefaultAsync(); 
        }
    }
}
