using CICTED.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Avaliador
{
    public class PrioridadeAvaliador
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public long UsuarioId { get; set; }
        [Required]
        public int Prioridade { get; set; }

    }
}
