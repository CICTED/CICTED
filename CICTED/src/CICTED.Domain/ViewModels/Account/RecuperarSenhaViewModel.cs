using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CICTED.Domain.ViewModels.Account
{
    public class RecuperarSenhaViewModel
    {
        [Required(ErrorMessage = "É necessário informar o email para a recuperação de senha.")]
        public string Email { get; set; }

        public string Message { get; set; }
        public bool Succeded { get; set; } = true;
    }
}
