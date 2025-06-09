using ItauChallenge.Application.DTO;
using ItauChallenge.Application.Interfaces;
using ItauChallenge.Core.Enums;
using ItauChallenge.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IPosicoesRepository _posicoesRepository;
        private readonly IOperacoesRepository _operacoesRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ClienteService(IPosicoesRepository posicoesRepository, IOperacoesRepository operacoesRepository, IUsuarioRepository usuarioRepository)
        {
            _posicoesRepository = posicoesRepository;
            _operacoesRepository = operacoesRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ClienteRanqueadoDto[]> GetTop10ClientesAsync(CriterioBuscaCliente criterio)
        {
            var todosUsuarios = await _usuarioRepository.GetAllAsync();
            var usuariosDict = todosUsuarios.ToDictionary(u => u.Id, u => u.Nome);

            switch (criterio)
            {
                case CriterioBuscaCliente.Posicao:
                    var todasPosicoes = await _posicoesRepository.GetAllAsync();
                    return (ClienteRanqueadoDto[])todasPosicoes
                        .GroupBy(p => p.UsuarioId)
                        .Select(g => new { UserId = g.Key, ValorTotal = g.Sum(p => p.Quantidade * p.PrecoMedio) })
                        .Take(10)
                        .Select((item, index) => new ClienteRanqueadoDto
                        {
                            Posicao = index + 1,
                            NomeCliente = usuariosDict.GetValueOrDefault(item.UserId, "N/A"),
                            Valor = item.ValorTotal
                        }).ToArray();

                case CriterioBuscaCliente.Corretagem:
                    var todasOperacoes = await _operacoesRepository.GetAllAsync();
                    return (ClienteRanqueadoDto[])todasOperacoes
                        .GroupBy(op => op.UsuarioId)
                        .Select(g => new { UserId = g.Key, TotalCorretagem = g.Sum(op => op.Corretagem) })
                        .OrderByDescending(x => x.TotalCorretagem)
                        .Take(10)
                        .Select((item, index) => new ClienteRanqueadoDto
                        {
                            Posicao = index + 1,
                            NomeCliente = usuariosDict.GetValueOrDefault(item.UserId, "N/A"),
                            Valor = item.TotalCorretagem
                        }).ToArray();

                break;
                         default:
                             throw new ArgumentOutOfRangeException(nameof(criterio), "Critério de busca não suportado.");
            }
        }
    }
}
