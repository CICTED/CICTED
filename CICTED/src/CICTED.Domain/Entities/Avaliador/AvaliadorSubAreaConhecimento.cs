using CICTED.Domain.Entities.Account;
using CICTED.Domain.Entities.Trabalho;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Avaliador
{
    public class AvaliadorSubAreaConhecimento
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public long SubAreaConhecimentoId { get; set; }

        
    }
}
