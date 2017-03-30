using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Cursos
{
    public class Cursos
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CursoNome { get; set; }
    }
}
