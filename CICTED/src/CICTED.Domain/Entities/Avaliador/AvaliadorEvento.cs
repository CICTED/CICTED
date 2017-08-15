using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Avaliador
{
    public class AvaliadorEvento
    {
        [Required]
        public int EventoId { get; set; }

        [Required]
        public long UsuarioId { get; set; }
    }
}
