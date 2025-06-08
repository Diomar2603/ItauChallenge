using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Entities
{
    [Table("dividendos")]
    public class Dividendo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("usr_id")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("atv_id")]
        public int AtivoId { get; set; }

        [Required]
        [Column("vlr_cota", TypeName = "decimal(18, 8)")]
        public decimal ValorPorCota { get; set; }

        [Required]
        [Column("qtd_base")]
        public int QuantidadeBase { get; set; }

        [Required]
        [Column("dt_pgto", TypeName = "date")]
        public DateTime DataPagamento { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual required Usuario Usuario { get; set; }

        [ForeignKey("AtivoId")]
        public virtual required Ativo Ativo { get; set; }
    }
}
