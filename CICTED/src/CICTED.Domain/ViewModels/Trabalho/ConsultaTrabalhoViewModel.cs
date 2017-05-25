using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Trabalho
{
    public class ConsultaTrabalho
    {
        public long Id { get; set; }

        public string Identificacao { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Titulo { get; set; }

        public int StatusTrabalhoId { get; set; }
        public int EventoId { get; set; }
        public int SubAreaConhecimentoId { get; set; }
        public int AreaConhecimentoId { get; set; }
    }
}
