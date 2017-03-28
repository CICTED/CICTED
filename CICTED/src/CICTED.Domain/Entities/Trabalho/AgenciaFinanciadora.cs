using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Trabalho
{
    public class AgenciaFinanciadora
    {
        [Key]
        public long Id { get; set; }

        public string Nome { get; set; }

    }
}
