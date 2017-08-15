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
        public string Sigla { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public string Objetivo { get; set; }

        [Required]
        public string PublicoAlvo { get; set; }

    }
}
