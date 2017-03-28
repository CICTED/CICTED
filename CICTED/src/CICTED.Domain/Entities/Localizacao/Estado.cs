using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Localizacao
{
    public class Estado
    {
        [Key]
        public long Id { get; set; }

        public string Nome { get; set; }

        public long PaisId { get; set; }
    }
}
