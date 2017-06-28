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
        public DateTime DataNascimento { get; set; }
        public string Nascimento { get; set; }
        public bool Genero { get; set; }
        public long EnderecoId { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public int Numero { get; set; }
        public long CidadeId { get; set; }
        public string CidadeNome { get; set; }
        public int EstadoId { get; set; }
        public string Sigla { get; set; }
        public bool FirstAccess { get; set; }
    }
}
