using ItauChallenge.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.Interfaces
{
    public interface ICotacaoService
    {
        Task<CotacaoDto> GetUltimaCotacaoAsync(string codigoAtivo);
    }
}
