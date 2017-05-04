using CICTED.Domain.Entities.Localizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Autor
{
    public class CadastroTrabalhoViewModel
    {
        public string Evento { get; set; }

        public int AreaConhecimento { get; set; }

        public int SubArea { get; set; }

        public int PeriodoApresentacao { get; set; }

        public bool TrabalhoFinanciado { get; set; }

        public int Agencia { get; set; }

        public string TextoCitacao { get; set; }

        public string CodigoCEP { get; set; }

        public List<Cidade> Cidades { get; set; }

        public List<Estado> Estado { get; set; }

        public List<long> Roles  { get; set; }

    }
}
