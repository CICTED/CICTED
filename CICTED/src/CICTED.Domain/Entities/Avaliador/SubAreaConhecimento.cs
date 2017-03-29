using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Trabalho
{
    public class SubAreaConhecimento
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public long AreaConhecimentoId { get; set; }

        
    }
}
