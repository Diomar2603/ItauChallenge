using ItauChallenge.Application.DTO;
using ItauChallenge.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteRanqueadoDto[]> GetTop10ClientesAsync(CriterioBuscaCliente criterio);
    }
}
