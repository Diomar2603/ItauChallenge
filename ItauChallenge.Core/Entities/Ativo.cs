using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Entities
{
    [Table("ativos")]
    public class Ativo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        [Column("cdg")]
        public required string Codigo { get; set; }

        [Required]
        [StringLength(255)]
        [Column("nome")]
        public required string Nome { get; set; }

        // Propriedades de Navegação
        public virtual ICollection<Operacao> Operacoes { get; set; } = new List<Operacao>();
        public virtual ICollection<Cotacao> Cotacoes { get; set; } = new List<Cotacao>();
        public virtual ICollection<Posicao> Posicoes { get; set; } = new List<Posicao>();
    }
}
