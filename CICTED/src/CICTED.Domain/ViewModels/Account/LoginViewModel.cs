using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [EmailAddress]
        public string EmailLogin { get; set; }

        [DataType(DataType.Password)]
        public string SenhaLogin { get; set; }
        [Display(Name = "Remember me?")]

        public bool RememberMe { get; set; }

        [EmailAddress]
        public string EmailCadastro { get; set; }

        [DataType(DataType.Password)]
        public string SenhaCadastro { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmSenhaCadastro { get; set; }
    }
}
