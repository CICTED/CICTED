using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Trabalho
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EventoNome { get; set; }

        [Required]
        public string SiglaEvento { get; set; }

        [Required]
        public string DescricaoEvento { get; set; }

        [Required]
        public string ObjetivoEvento { get; set; }

        [Required]
        public string PublicoAlvoEvento { get; set; }

    }
}
