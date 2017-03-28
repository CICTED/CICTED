using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Account
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string EmailSecundario { get; set; }
        public DateTime DataCadastro { get; set; }
        public string CPF { get; set; }
        public string Documento { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public bool Genero { get; set; }
        public string Celular { get; set; }
        public bool Estudante { get; set; }
        public bool Bolsista { get; set; }
    }
}
