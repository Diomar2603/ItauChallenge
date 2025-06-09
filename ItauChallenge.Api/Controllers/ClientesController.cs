using ItauChallenge.Application.Interfaces;
using ItauChallenge.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ItauChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet("top10")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetTop10Clientes([FromQuery] CriterioBuscaCliente por)
        {
            var topClientes = await _clienteService.GetTop10ClientesAsync(por);

            return Ok(new { criterio = por.ToString().ToUpper(), clientes = topClientes });
        }
    }
}