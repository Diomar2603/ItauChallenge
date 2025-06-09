using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.DTO
{
    public class ClienteRanqueadoDto
    {
        public int Posicao { get; set; }
        public string NomeCliente { get; set; }
        public decimal Valor { get; set; }
    }
}
