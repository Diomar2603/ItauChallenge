using ItauChallenge.Application.DTO;
using ItauChallenge.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.Services
{
    public class CotacaoService : ICotacaoService
    {
        public Task<CotacaoDto> GetUltimaCotacaoAsync(string codigoAtivo)
        {
            throw new NotImplementedException();
        }
    }
}
