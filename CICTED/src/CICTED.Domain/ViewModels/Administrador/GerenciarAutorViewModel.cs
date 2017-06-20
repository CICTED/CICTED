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
        public string TrabalhoIdentificacao { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public bool FirstAccess { get; set; }
    }
}
