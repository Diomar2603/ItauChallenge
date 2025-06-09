using ItauChallenge.Core.Entities;
using ItauChallenge.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Infrastructure.Data.Repositories
{
    public class CotacoesRepository : ICotacoesRepository
    {
        public void AdicionarAsync(Cotacao cotacao)
        {
            throw new NotImplementedException();
        }

        Task<bool> ICotacoesRepository.ExisteAssync(int ativoId, DateTime dataHora)
        {
            throw new NotImplementedException();
        }
    }
}
