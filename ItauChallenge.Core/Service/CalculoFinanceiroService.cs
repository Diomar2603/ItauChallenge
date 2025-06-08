using ItauChallenge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Service
{
    public class CalculoFinanceiroService
    {
        /// <summary>
        /// Calcula o preço médio ponderado de uma lista de operações de compra.
        /// </summary>
        /// <param name="compras">Uma coleção de operações de compra.</param>
        /// <returns>O preço médio ponderado.</returns>
        /// <exception cref="ArgumentNullException">Lançada se a lista de compras for nula.</exception>
        /// <exception cref="ArgumentException">Lançada se a lista de compras estiver vazia ou contiver dados inválidos.</exception>
        public decimal CalcularMediaPonderada(Operacao[] compras)
        {
            if (compras == null)
            {
                throw new ArgumentNullException(nameof(compras), "A lista de compras não pode ser nula.");
            }

            if (!compras.Any())
            {
                return 0;
            }

            decimal custoTotal = 0;
            int quantidadeTotal = 0;

            foreach (var compra in compras)
            {
                if (compra.Quantidade <= 0)
                {
                    throw new ArgumentException($"A operação com ID {compra.Id} tem uma quantidade inválida (<=0): {compra.Quantidade}.");
                }
                if (compra.PrecoUnitario < 0)
                {
                    throw new ArgumentException($"A operação com ID {compra.Id} tem seu preço unitário inválido (<=0): {compra.PrecoUnitario}.");
                }

                custoTotal += compra.Quantidade * compra.PrecoUnitario;
                quantidadeTotal += compra.Quantidade;
            }

            if (quantidadeTotal == 0)
            {
                return 0;
            }

            return custoTotal / quantidadeTotal;
        }
    }
}
