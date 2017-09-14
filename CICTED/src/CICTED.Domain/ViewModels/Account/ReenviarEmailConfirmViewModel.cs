using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CICTED.Domain.ViewModels.Account
{
    public class ReenviarEmailConfirmViewModel
    {
        [Required(ErrorMessage = "É necessário informar o email para a reenvio de confirmação.")]
        public string Email { get; set; }
        public string Message { get; set; }
        public bool Succeded { get; set; } = true;
    }
}
