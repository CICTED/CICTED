﻿using CICTED.Domain.Entities.Account;
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
        public long StatusUsuarioId { get; set; }
        [Required]
        public long UserId { get; set; }

        public StatusUsuario StatusUsuario { get; set; }
        public ApplicationUser User { get; set; }
    }
}