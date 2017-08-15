using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Account
{
    public class EnderecoViewModel
    {
        public long EnderecoId { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public int Numero { get; set; }
        public long CidadeId { get; set; }
        public string CidadeNome { get; set; }
        public int EstadoId { get; set; }
        public string Sigla { get; set; }
    }
}
