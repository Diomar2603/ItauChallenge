using ItauChallenge.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Entities
{
    [Table("operacoes")]
    public class Operacao
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("usr_id")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("atv_id")]
        public int AtivoId { get; set; }

        [Required]
        [Column("qtd")]
        public int Quantidade { get; set; }

        [Required]
        [Column("prc_unt", TypeName = "decimal(18, 8)")]
        public decimal PrecoUnitario { get; set; }

        [Required]
        [Column("tipo")]
        public TipoOperacao Tipo { get; set; }

        [Required]
        [Column("corretagem", TypeName = "decimal(10, 2)")]
        public decimal Corretagem { get; set; }

        [Required]
        [Column("dt_hr", TypeName = "datetime(3)")]
        public DateTime DataHora { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual required Usuario Usuario { get; set; }

        [ForeignKey("AtivoId")]
        public virtual required Ativo Ativo { get; set; }
    }
}
