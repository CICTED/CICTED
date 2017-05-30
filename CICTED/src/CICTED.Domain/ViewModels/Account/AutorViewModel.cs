using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Account
{
    public class AutorViewModel
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public string Email { get; set; }

        public int StatusId { get; set; }

        public bool Orientador { get; set; }

        public int InstituicaoId { get; set; }

        public string Instituicao { get; set; }
    }
}
