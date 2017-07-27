using CICTED.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Trabalho
{
    public class AutoresViewModel
    {
        public long Id { get; set; }

        public int EventoId { get; set; }

        public List<string> AlunosNome { get; set; }

        public AutorViewModel AutorPrincipal { get; set; }

        public List<AutorViewModel> Coautores { get; set; }

        public AutorViewModel Orientador { get; set; }

        //take ids
        public List<long> CoautoresId { get; set; }

        public long OrientadorId { get; set; }

        public long OrientadorEmail { get; set; }

        public List<long> CoautoresEmail { get; set; }

        //take info alunos
        public List<AlunoViewModel> AlunosInfo { get; set; }
    }
}
