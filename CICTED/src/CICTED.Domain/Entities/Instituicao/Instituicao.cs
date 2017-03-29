using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Instituicao
{
    public class Instituicao
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string InstituicaoNome { get; set; }
    }
}
