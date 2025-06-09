using ItauChallenge.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.Interfaces
{
    public interface IPortfolioService
    {
        public Task<UsuarioPortfolioDto> GetPortifolioUsuarioAsync(int userId);

        public Task<PrecoMedioDto> GetPrecoMedioPorAtivoAsync(int usuarioId, string codigoAtivo);

        public Task<PosicaoConsolidadaDto> GetPosicaoConsolidadaAsync(int usuarioId);
    }
}
