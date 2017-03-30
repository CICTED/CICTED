using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Atividades
{
    public class PublicoAtividade
    {
        [Required]
        public int PublicoAlvoId { get; set; }
         
        [Required]
        public long AtividadeAcademicaId { get; set; }
    }
}
