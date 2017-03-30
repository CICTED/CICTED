using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Atividades
{
    public class TipoAtividade
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string TipoAtividadeNome { get; set; }
    }
}
