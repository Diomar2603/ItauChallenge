using ItauChallenge.Application.DTO;
using ItauChallenge.Application.Interfaces;
using ItauChallenge.Core.Enums;
using ItauChallenge.Core.Interfaces;
using ItauChallenge.Core.Service;
using ItauChallenge.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IOperacoesRepository _operacoesRepository;
        private readonly IPosicoesRepository _posicoesRepository;
        private readonly IAtivosRepository _ativosRepository;
        private readonly ICalculoFinanceiroService _calculadorFinanceiro;


        public PortfolioService(IOperacoesRepository operacaoRepo, IPosicoesRepository posicaoRepo, 
                                IAtivosRepository ativoRepo, ICalculoFinanceiroService calculoFinanceiroService)
        {
            _operacoesRepository = operacaoRepo;
            _posicoesRepository = posicaoRepo;
            _ativosRepository = ativoRepo;
            _calculadorFinanceiro = calculoFinanceiroService;
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

        public async Task<PosicaoConsolidadaDto> GetPosicaoConsolidadaAsync(int usuarioId)
        {
            var posicoes =  await _posicoesRepository.GetByIdUsuarioAsync(usuarioId);
            if (!posicoes.Any()) return null;

            var lucroPrejuizoGlobal = posicoes.Sum(p => p.LucroPrejuizo);

            var valorAtualDaCarteira = posicoes.Sum(p => p.Quantidade * p.PrecoMedio) + lucroPrejuizoGlobal;

            return new PosicaoConsolidadaDto
            {
                ClienteId = usuarioId,
                LucroPrejuizoGlobal = lucroPrejuizoGlobal,
                ValorAtualDaCarteira = valorAtualDaCarteira,
                ValorTotalInvestido = posicoes.Sum(p => p.Quantidade * p.PrecoMedio)
            };
        }

        public async Task<PrecoMedioDto> GetPrecoMedioPorAtivoAsync(int usuarioId, string codigoAtivo)
        {
            var ativo = await _ativosRepository.GetByCodigoAsync(codigoAtivo);

            if (ativo == null) return null;

            var compras = await _operacoesRepository.GetComprasPorUsuarioEAtivoAsync(usuarioId, ativo.Id);
            if (!compras.Any()) return null;

            // Usa o serviço de cálculo que criamos na Atividade 4
            var precoMedio = _calculadorFinanceiro.CalcularMediaPonderadaAtivo(compras.ToArray());

            return new PrecoMedioDto
            {
                UsuarioId = usuarioId,
                CodigoAtivo = codigoAtivo,
                PrecoMedio = precoMedio
            };
        }
    }
}
