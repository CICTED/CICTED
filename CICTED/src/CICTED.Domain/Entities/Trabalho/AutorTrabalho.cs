using CICTED.Domain.Entities.Account;
using CICTED.Domain.Entities.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Trabalho
{
    public class AutorTrabalho
    {
        [Required]
        public int StatusUsuarioId { get; set; }
        [Required]
        public long UsuarioId { get; set; }
      
    }
}
