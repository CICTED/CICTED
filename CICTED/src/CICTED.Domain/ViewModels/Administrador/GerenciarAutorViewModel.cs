using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Administrador
{
    public class GerenciarAutor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Identificacao { get; set; }
        public string PhoneNumber { get; set; }
        public bool FirstAccess { get; set; }
    }
}
