﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Localizacao
{
    public class Cidade
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string CidadeNome { get; set; }

        [Required]
        public int EstadoId { get; set; }
    }
}
