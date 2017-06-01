using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Account
{
    public class AlterarSenhaViewModel
    {
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
        public string ConfirmarSenha { get; set; }
    }
}
