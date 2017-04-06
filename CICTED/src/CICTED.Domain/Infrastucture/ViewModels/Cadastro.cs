using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.ViewModels
{
    public class Cadastro
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public string ConfirmaSenha { get; set; }
    }
}
