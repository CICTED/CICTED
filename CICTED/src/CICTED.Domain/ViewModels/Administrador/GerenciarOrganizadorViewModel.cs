using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Administrador
{
    public class GerenciarOrganizador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Avaliador { get; set; }
        public string CPF { get; set; }
        public DateTime Nascimento { get; set; }
        public bool Genero { get; set; }
        public string Endereco { get; set; }
        public int InstituicaoId { get; set; }
        public string Instituicao { get; set; }
        public bool FirstAccess { get; set; }
    }
}
