using CICTED.Crosscutting.Validations;
using CICTED.Domain.Entities.Localizacao;
using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Autor
{
    public class CadastroTrabalhoViewModel
    {        

        public Evento Evento { get; set; }

        public int EventoId { get; set; }

        public string NomeEscola { get; set; }

        public string CidadeEscola { get; set; }

        public string TelefoneEscola { get; set; }

        public List<AreaConhecimento> AreasConhecimento { get; set; }

        public int AreaConhecimento { get; set; }

        public int SubAreaId { get; set; }

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

        public string AlunoNome { get; set; }
        
        [MaxWords(20, ErrorMessage = "máximo 20 palavras")]
        [MinWords(5, ErrorMessage = "mínimo 5 palavras")]
        public string Titulo { get; set; }

        public string Resumo { get; set; }

        public string Introducao { get; set; }

        public string Objetivo { get; set; }

        [MaxWords(150, ErrorMessage = "máximo 150 palavras")]
        [MinWords(20, ErrorMessage = "mínimo 20 palavras")]
        public string Metodologia { get; set; }

        [MaxWords(20, ErrorMessage = "máximo 150 palavras")]
        [MinWords(5, ErrorMessage = "mínimo 20 palavras")]
        public string Resultados { get; set; }

        [MaxWords(20, ErrorMessage = "máximo 100 palavras")]
        [MinWords(5, ErrorMessage = "mínimo 20 palavras")]
        public string Conclusao { get; set; }

        [MaxWords(20, ErrorMessage = "máximo 100 palavras")]
        [MinWords(5, ErrorMessage = "mínimo 20 palavras")]
        public string Referencias { get; set; }

        [MaxWords(20, ErrorMessage = "máximo 6 palavras")]
        [MinWords(5, ErrorMessage = "mínimo 3 palavras")]
        public String PalavraChave { get; set; }
        public List<string> PalavrasChave { get; set; }

        public long ArtigoId { get; set; }

        public DateTime DataCadastro { get; set; }

        public List<Cidade> Cidades { get; set; }

        public List<Estado> Estado { get; set; }

        public List<long> Roles { get; set; }

        public string ReturnMenssagem { get; set; }

        public List<AutorTrabalho> autores { get; set; }
    }
}
