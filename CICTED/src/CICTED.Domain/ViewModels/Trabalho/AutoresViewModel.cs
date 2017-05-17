using CICTED.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Trabalho
{
    public class AutoresViewModel
    {
        public AutorViewModel AutorPrincipal { get; set; }

        public List<AutorViewModel> Coautores { get; set; }

        public AutorViewModel Orientador { get; set; }
    }
}
