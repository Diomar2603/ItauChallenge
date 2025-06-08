using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Entities
{
    [Table("posicoes")]
    public class Posicao
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
        [Column("qtd")]
        public int Quantidade { get; set; }

        [Required]
        [Column("prc_medio", TypeName = "decimal(18, 2)")]
        public decimal PrecoMedio { get; set; }

        [Required]
        [Column("pl", TypeName = "decimal(18, 2)")]
        public decimal LucroPrejuizo { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual required Usuario Usuario { get; set; }

        [ForeignKey("AtivoId")]
        public virtual required Ativo Ativo { get; set; }
    }
}
