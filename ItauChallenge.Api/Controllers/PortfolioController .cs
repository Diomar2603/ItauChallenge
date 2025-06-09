using ItauChallenge.Application.DTO;
using ItauChallenge.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItauChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        /// <summary>
        /// Consulta o preço médio de um ativo específico para um usuário.
        /// </summary>
        [HttpGet("usuarios/{usuarioId}/ativos/{codigoAtivo}/preco-medio")]
        [ProducesResponseType(typeof(PrecoMedioDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPrecoMedioPorAtivo(int usuarioId, string codigoAtivo)
        {
            var precoMedio = await _portfolioService.GetPrecoMedioPorAtivoAsync(usuarioId, codigoAtivo);

            if (precoMedio == null)
            {
                return NotFound($"Nenhum dado encontrado para o usuário {usuarioId} e ativo {codigoAtivo}.");
            }

            return Ok(precoMedio);
        }

        /// <summary>
        /// Consulta a posição consolidada (carteira) de um cliente.
        /// </summary>
        [HttpGet("usuarios/{usuarioId}/posicao-consolidada")]
        [ProducesResponseType(typeof(PosicaoConsolidadaDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPosicaoConsolidada(int usuarioId)
        {
            var posicao = await _portfolioService.GetPosicaoConsolidadaAsync(usuarioId);

            if (posicao == null)
            {
                return NotFound($"Nenhuma posição encontrada para o usuário {usuarioId}.");
            }

            return Ok(posicao);
        }
    }
}
