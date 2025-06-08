using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.DTO
{
    public class InvestimentoPorAtivoDto
    {
        public string CodigoAtivo { get; set; }

        public decimal ValorTotalInvestido { get; set; }

        public int QuantidadeAtual { get; set; }

        public decimal PrecoMedio { get; set; }
    }
}
