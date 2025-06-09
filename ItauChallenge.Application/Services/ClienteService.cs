using ItauChallenge.Application.DTO;
using ItauChallenge.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.Services
{
    public class ClienteService : IClienteService
    {
        public Task<ClienteRanqueadoDto[]> GetTop10ClientesAsync(string criterio)
        {
            throw new NotImplementedException();
        }
    }
}
