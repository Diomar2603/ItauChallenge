using ItauChallenge.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItauChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CorretoraController : ControllerBase
    {
        private readonly ICorretoraService _corretoraService;

        public CorretoraController(ICorretoraService corretoraService)
        {
            _corretoraService = corretoraService;
        }

        [HttpGet("receita-total")] 
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> GetReceitaTotal()
        {
            var receita = await _corretoraService.GetReceitaTotalDeCorretagemAsync();

            return Ok(new { receitaTotalCorretagem = receita });
        }
    }
}
