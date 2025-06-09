using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.DTO
{
    public class PrecoMedioDto
    {
        public int UsuarioId { get; set; }
        public string CodigoAtivo { get; set; }
        public decimal PrecoMedio { get; set; }
    }
}
