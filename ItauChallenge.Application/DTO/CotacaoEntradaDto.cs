using ItauChallenge.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Application.DTO
{
    public class CotacaoEntradaDto
    {
        public string CodigoDoAtivo {  get; set; }

        public decimal PrecoUnitario { get; set; }

        public DateTime DataHora { get; set; }

    }
}
