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

        public long TipoAtividadeId { get; set; }

        public long StatusTrabalhoId { get; set; }

        public long UserId { get; set; }

        public string Departamento { get; set; }

        public DateTime DataApresentacao { get; set; }

        public string Identificacao { get; set; }

        public int QtdPublico { get; set; }

        public string Titulo { get; set; }

        public string Resumo { get; set; }

        public string Observacao { get; set; }

        public DateTime DataCadastro { get; set; }

      
    }
}
