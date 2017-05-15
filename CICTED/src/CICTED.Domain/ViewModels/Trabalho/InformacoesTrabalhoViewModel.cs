using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Trabalho
{
    public class InformacoesTrabalhoViewModel
    {

        public long Id { get; set; }

        public string Titulo { get; set; }

        public string Introducao { get; set; }

        public string Metodologia { get; set; }

        public string Resultado { get; set; }

        public string Resumo { get; set; }

        public string Conclusao { get; set; }

        public string Referencia { get; set; }

        public string NomeEscola { get; set; }

        public string TelefoneEscola { get; set; }

        public string CidadeEscola { get; set; }

        public string Identificacao { get; set; }

        public DateTime DataSubmissao { get; set; }

        public DateTime DataCadastro { get; set; }

        public string TextoFinanciadora { get; set; }

        public string CodigoCEP { get; set; }

        public string EventoNome { get; set; }

        public List<string> palavrasChave { get; set; }

        public CoautorViewModel orientador { get; set; }

        public string AreaConhecimento { get; set; }

        public string SubArea { get; set; }

        public string Status { get; set; }

        public int StatusTrabalhoId { get; set; }

        public List<CoautorViewModel> autores { get; set; }
    }


}
