using ItauChallenge.Application.DTO;
using ItauChallenge.Application.Interfaces;
using ItauChallenge.Core.Interfaces;
using ItauChallenge.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.Services
{
    public class CotacaoService : ICotacaoService
    {
        private readonly ICotacoesRepository _cotacoesRepository;
        private readonly IAtivosRepository _ativosRepository;

        public CotacaoService(ICotacoesRepository cotacoesRepository, IAtivosRepository ativosRepository)
        {
            _cotacoesRepository = cotacoesRepository;
            _ativosRepository = ativosRepository;
        }
        public async Task<CotacaoDto> GetUltimaCotacaoAsync(string codigoAtivo)
        {
            var ativo = await _ativosRepository.GetByCodigoAsync(codigoAtivo);
            if (ativo == null)
            {
                return null;
            }

            var ultimaCotacao = await _cotacoesRepository.GetUltimaPorAtivoIdAsync(ativo.Id);
            if (ultimaCotacao == null)
            {
                return null;
            }

            return new CotacaoDto
            {
                CodigoAtivo = ativo.Codigo,
                PrecoUnitario = ultimaCotacao.PrecoUnitario,
                DataHora = ultimaCotacao.DataHora
            };
        }
    }
}
