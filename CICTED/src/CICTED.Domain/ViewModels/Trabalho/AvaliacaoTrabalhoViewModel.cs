using CICTED.Domain.Entities.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Trabalho
{
    public class AvaliacaoTrabalhoViewModel
    {
        public List<Evento> Eventos { get; set; }
        public int EventoId { get; set; }        
        public string IdentificadorTrabalho { get; set; }
        public string TituloTrabalho { get; set; }
        public string SubArea { get; set; }
        public string Avaliador { get; set; }
        public float Nota { get; set; }
        public float NotaResumo { get; set; }
        public float NotaMetodologia { get; set; }
        public float NotaResultado { get; set; }
        public float NotaObjetivo { get; set; }
        public bool Favorito { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public int TipoAvaliacao { get; set; }
    }
}
