using ItauChallenge.Core.Entities;
using ItauChallenge.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ItauChallenge.Core.Service
{
    public class CalculoFinanceiroService : ICalculoFinanceiroService
    {
        public decimal CalcularMediaPonderadaAtivo(Operacao[] compras)
        {
            if (compras == null)
            {
                throw new ArgumentNullException(nameof(compras), "A lista de compras não pode ser nula.");
            }

            var listaDeCompras = compras.ToList();

            if (!listaDeCompras.Any())
            {
                return 0;
            }

            var gruposPorAtivo = listaDeCompras.GroupBy(c => c.AtivoId);

            if (gruposPorAtivo.Count() > 1)
            {
                var idsDosAtivos = string.Join(", ", gruposPorAtivo.Select(g => g.Key));
                throw new ArgumentException($"A lista de compras deve conter operações de apenas um ativo, mas foram encontrados os seguintes IDs: {idsDosAtivos}.");
            }

            decimal custoTotal = 0;
            int quantidadeTotal = 0;

            foreach (var compra in listaDeCompras)
            {
                if (compra.Quantidade <= 0)
                {
                    throw new ArgumentException($"A operação com ID {compra.Id} tem uma quantidade inválida: {compra.Quantidade}.");
                }
                if (compra.PrecoUnitario < 0)
                {
                    throw new ArgumentException($"A operação com ID {compra.Id} tem um preço unitário inválido: {compra.PrecoUnitario}.");
                }

                custoTotal += compra.Quantidade * compra.PrecoUnitario;
                quantidadeTotal += compra.Quantidade;
            }

            return custoTotal / quantidadeTotal;
        }
    }
}