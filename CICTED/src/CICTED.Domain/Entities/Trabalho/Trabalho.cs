using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Trabalho
{
    public class Trabalho
    {
        [Key]
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

        [Required]
        public DateTime DataCadastro { get; set; }

        public string TextoFinanciadora { get; set; }

        public string CodigoCEP { get; set; }

        public int AgenciaFinanciadoraId { get; set; }

        [Required]
        public int EventoId { get; set; }

        public long ArtigoId { get; set; }

        public int SubAreaConhecimentoId { get; set; }

        public int StatusTrabalhoId { get; set; }
    }
}
