using CICTED.Domain.Entities.Account;
using CICTED.Domain.Entities.Trabalho;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Atividades
{
    public class AtividadeAcademica
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long TipoAtividadeId { get; set; }

        [Required]
        public long StatusTrabalhoId { get; set; }

        [Required]
        public long UsuarioId { get; set; }

        public string Departamento { get; set; }

        public DateTime DataApresentacao { get; set; }

        [Required]
        public string Identificacao { get; set; }

        public int QtdPublico { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Resumo { get; set; }

        public string Observacao { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }

      
    }
}
