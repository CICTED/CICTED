using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Administrador
{
    public class GerenciarAvaliador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Evento { get; set; }
        public int EventoId { get; set; }
        public string SubAreaConhecimento { get; set; }
        public int SubAreaConhecimentoId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool FirstAccess { get; set; }
    }
}
