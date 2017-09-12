using System;
using System.Collections.Generic;
using System.Text;

namespace CICTED.Domain.ViewModels.Avaliador
{
    public class AvaliaTrabalhoViewModel
    {
        public string IdentificacaoTrabalho { get; set; }
        public int StatusTrabalhoId { get; set; }
        public long UsuarioId { get; set; }
        public long TrabalhoId { get; set; }
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
