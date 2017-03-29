using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Trabalho
{
    public class AvaliacaoTrabalho
    {
        [Key]
        public long Id { get; set; }

        public long TrabalhoId { get; set; }

        public long StatusTrabalhoId { get; set; }

        public long UserId { get; set; }

        public double Nota { get; set; }

        public double NotaResumo { get; set; }

        public double NotaMetodologia { get; set; }

        public double NotaResultado { get; set; }

        public double NotaObjetivo { get; set; }

        public bool Favorito { get; set; }

        public string Comentario { get; set; }

        public DateTime DataAvaliacao { get; set; }
    }
}
