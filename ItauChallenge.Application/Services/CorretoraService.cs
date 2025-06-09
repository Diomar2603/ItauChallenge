using ItauChallenge.Application.Interfaces;
using ItauChallenge.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.Services
{
    public class CorretoraService : ICorretoraService
    {
        private readonly IOperacoesRepository _operacoesRepository;

        public CorretoraService(IOperacoesRepository operacoesRepository)
        {
            _operacoesRepository = operacoesRepository;
        }
        public async Task<decimal> GetReceitaTotalDeCorretagemAsync()
        {
            return await _operacoesRepository.GetSomaTotalCorretagemAsync();
        }
    }
}
