using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ItauChallenge.Core.Entities
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("nome")]
        public required string Nome { get; set; }

        [Required]
        [StringLength(255)]
        [Column("email")]
        public required string Email { get; set; }

        [Required]
        [Column("prct_corretagem", TypeName = "decimal(5, 4)")]
        public required decimal PercentualCorretagem { get; set; }

        public virtual ICollection<Operacao> Operacoes { get; set; } = new List<Operacao>();
        public virtual ICollection<Posicao> Posicoes { get; set; } = new List<Posicao>();
        public virtual ICollection<Dividendo> Dividendos { get; set; } = new List<Dividendo>();
    }
}
