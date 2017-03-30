using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Atividades
{
    public class Ministrantes
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string MinistranteNome { get; set; }
    }
}
