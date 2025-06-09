using ItauChallenge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Interfaces
{
    public interface ICotacoesRepository 
    {
        public Task<Boolean> ExisteAsync(int ativoId, DateTime dataHora);
        public Task<Cotacao> AdicionarAsync(Cotacao cotacao);
        public Task<Cotacao> GetUltimaPorAtivoIdAsync(int ativoId);

    }
}
