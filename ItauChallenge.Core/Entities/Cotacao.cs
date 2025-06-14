﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Entities
{
    [Table("cotacoes")]
    public class Cotacao
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("atv_id")]
        public int AtivoId { get; set; }

        [Required]
        [Column("prc_unt", TypeName = "decimal(18, 2)")]
        public decimal PrecoUnitario { get; set; }

        [Required]
        [Column("dt_hr", TypeName = "datetime(3)")]
        public DateTime DataHora { get; set; }

        public Cotacao(int ativoId, decimal precoUnitario, DateTime dataHora)
        {
            AtivoId = ativoId;
            PrecoUnitario = precoUnitario;
            DataHora = dataHora;
        }

        [ForeignKey("AtivoId")]
        public virtual Ativo Ativo { get; set; }
    }
}
