using ItauChallenge.Application.DTO;
using ItauChallenge.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItauChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CotacoesController : ControllerBase
    {
        private readonly ICotacaoService _cotacaoService;

        public CotacoesController(ICotacaoService cotacaoService)
        {
            _cotacaoService = cotacaoService;
        }

        [HttpGet("{codigoAtivo}/ultima")] 
        [ProducesResponseType(typeof(CotacaoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUltimaCotacao(string codigoAtivo)
        {
            var cotacao = await _cotacaoService.GetUltimaCotacaoAsync(codigoAtivo);

            if (cotacao == null)
            {
                return NotFound($"Nenhuma cotação encontrada para o ativo {codigoAtivo}.");
            }

            return Ok(cotacao);
        }
    }
}
