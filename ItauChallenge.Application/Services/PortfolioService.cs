using ItauChallenge.Application.DTO;
using ItauChallenge.Core.Enums;
using ItauChallenge.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.Services
{
    public class PortfolioService
    {
        private readonly IOperacoesRepository _operacoesRepository;
        private readonly IPosicoesRepository _posicoesRepository;
        private readonly IAtivosRepository _ativosRepository;

        public PortfolioService(IOperacoesRepository operacaoRepo, IPosicoesRepository posicaoRepo, IAtivosRepository ativoRepo)
        {
            _operacoesRepository = operacaoRepo;
            _posicoesRepository = posicaoRepo;
            _ativosRepository = ativoRepo;
        }

        public async Task<UsuarioPortfolioDto> GetPortifolioUsuarioAsync(int userId)
        {
            var operacoes = await _operacoesRepository.GetByIdUsuarioAsync(userId);
            var posicoes = await _posicoesRepository.GetByIdUsuarioAsync(userId);

            var totalBrokerage = operacoes.Sum(op => op.Corretagem);

            var investimentoByAtivoDto = new List<InvestimentoPorAtivoDto>();
            var operacoesPorAtivo = operacoes.GroupBy(op => op.AtivoId);

            foreach (var group in operacoesPorAtivo)
            {
                var ativoId = group.Key;
                var posicao = posicoes.FirstOrDefault(p => p.AtivoId == ativoId);
                var ativo = await _ativosRepository.GetByIdAsync(ativoId);

                var totalInvestidoAtivo = group
                    .Where(op => op.Tipo.Equals(TipoOperacao.Compra))
                    .Sum(op => op.Quantidade * op.PrecoUnitario);

                investimentoByAtivoDto.Add(new InvestimentoPorAtivoDto
                {
                    CodigoAtivo = ativo.Codigo, 
                    ValorTotalInvestido = totalInvestidoAtivo,
                    QuantidadeAtual = posicao?.Quantidade ?? 0,
                    PrecoMedio = posicao?.PrecoMedio ?? 0
                });
            }

            var lucroPrejuizoGlobal = posicoes.Sum(p => p.LucroPrejuizo);

            return new UsuarioPortfolioDto
            {
                ClienteId = userId,
                LucroPrejuizoGlobal = lucroPrejuizoGlobal,
                TotalCorretagemPaga = totalBrokerage,
                Posicoes = investimentoByAtivoDto
            };
        }
    }
}
