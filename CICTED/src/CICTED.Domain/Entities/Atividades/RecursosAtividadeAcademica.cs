using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Atividades
{
    public class RecursosAtividadeAcademica
    {
        [Required]
        public int RecursosId { get; set; }

        [Required]
        public long AtividadeAcademicaId { get; set; }
       
    }
}
