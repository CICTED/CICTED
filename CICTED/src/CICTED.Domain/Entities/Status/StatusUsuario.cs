using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Status
{
    public class StatusUsuario
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }
    }
}
