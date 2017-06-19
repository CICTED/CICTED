using CICTED.Crosscutting.Validations;
using CICTED.Domain.Entities.Localizacao;
using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Autor
{
    public class CadastroTrabalhoViewModel
    {        

        public Evento Evento { get; set; }

        public int EventoId { get; set; }

        public string EventoNome { get; set; }

        public string NomeEscola { get; set; }

        public string CidadeEscola { get; set; }

        public string TelefoneEscola { get; set; }

        public List<AreaConhecimento> AreasConhecimento { get; set; }

        public int AreaConhecimento { get; set; }

        public int SubAreaId { get; set; }

        public string SubAreaNome { get; set; }

        public List<PeriodoApresentacao> Periodos { get; set; }        

        public int PeriodoApresentacao { get; set; }

        public bool TrabalhoFinanciado { get; set; }

        public List<AgenciaFinanciadora> Agencias { get; set; }

        public int AgenciaId { get; set; }

        public string TextoCitacao { get; set; }        

        public string CodigoCEP { get; set; }

        public AutorViewModel AutorPrincipal { get; set; }
        
        public List<AutorViewModel> Coautores { get; set; }

        public AutorViewModel Orientador { get; set; } 

        public List<AlunoTrabalho> Alunos { get; set; }

        public List<string> AlunosNome { get; set; }
        
        [MinMaxWords(5, 20, ErrorMessage = "mínimo 5 palavras, máximo 20 palavras")]
        public string Titulo { get; set; }

        public string Resumo { get; set; }

        [MinMaxWords(20, 150, ErrorMessage = "mínimo 20 palavras, máximo 150 palavras")]
        public string Introducao { get; set; }

        public string Objetivo { get; set; }

        [MinMaxWords(50, 300, ErrorMessage = "mínimo 50 palavras, máximo 300 palavras")]
        public string Metodologia { get; set; }

        [MinMaxWords(20, 150, ErrorMessage = "mínimo 20 palavras, máximo 150 palavras")]
        public string Resultados { get; set; }

        [MinMaxWords(20, 100, ErrorMessage = "mínimo 20 palavras, máximo 100 palavras")]
        public string Conclusao { get; set; }

        [MinMaxWords(20, 100, ErrorMessage = "mínimo 20 palavras, máximo 100 palavras")]
        public string Referencias { get; set; }

        [MinMaxWords(3, 6, ErrorMessage = "mínimo 3 palavras, máximo 6 palavras")]
        public string PalavraChave { get; set; }

        public List<string> PalavrasChave { get; set; }

        public long ArtigoId { get; set; }

        public DateTime DataCadastro { get; set; }

        public List<Cidade> Cidades { get; set; }

        public List<Estado> Estado { get; set; }

        public int StatusTrabalhoId { get; set; }

        public List<long> Roles { get; set; }

        public string ReturnMenssagem { get; set; }

        public List<AutorTrabalho> Autores { get; set; }

        //take ids
        public List<long> CoautoresId { get; set; }

        public long OrientadorId { get; set; }

        public long OrientadorEmail { get; set; }

        public List<long> CoautoresEmail { get; set; }
    }
}
