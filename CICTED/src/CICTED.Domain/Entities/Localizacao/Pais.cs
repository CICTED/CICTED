using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Localizacao
{
    public class Pais
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PaisNome { get; set; }

        [Required]
        public string Sigla { get; set; }
    }
}
