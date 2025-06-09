using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.DTO
{
    public class CotacaoDto
    {
        public string CodigoAtivo { get; set; }
        public decimal PrecoUnitario { get; set; }
        public DateTime DataHora { get; set; }
    }
}
