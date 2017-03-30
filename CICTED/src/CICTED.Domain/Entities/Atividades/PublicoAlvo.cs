using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Atividades
{
    public class PublicoAlvo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Publico { get; set; }
    }
}
