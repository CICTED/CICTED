using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CICTED.Domain.ViewModels.Account
{
    public class ForgotPasswordConfirmationViewModel
    {
        [Required(ErrorMessage = "É necessário fornecer o email para recuperar a senha.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "É necessário fornecer o token de confirmação para alterar a senha.")]
        public string Token { get; set; }
        [Required(ErrorMessage = "Forneça uma nova senha.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 digitos.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "É necessário fornecer a confirmação da senha.")]
        public string ConfirmNewPassword { get; set; }

        public string Message { get; set; } = string.Empty;
        public bool Succeeded { get; set; }
    }
}
