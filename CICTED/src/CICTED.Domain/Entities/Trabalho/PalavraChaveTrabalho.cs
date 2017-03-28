using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Trabalho
{
    public class PalavraChaveTrabalho
    {
        [Required]
        public long PalavraChaveId { get; set; }
        [Required]
        public long TrabalhoId { get; set; }

        public PalavraChave PalavraChave { get; set; }
        public Trabalho Trabalho { get; set; }
    }
}
