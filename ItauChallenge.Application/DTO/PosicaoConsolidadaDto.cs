using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.DTO
{
    public class PosicaoConsolidadaDto
    {
        public int ClienteId { get; set; }
        public decimal ValorTotalInvestido { get; set; }
        public decimal ValorAtualDaCarteira { get; set; }
        public decimal LucroPrejuizoGlobal { get; set; }
    }
}
