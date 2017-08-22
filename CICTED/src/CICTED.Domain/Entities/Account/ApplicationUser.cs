using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CICTED.Domain.Entities.Cursos;

namespace CICTED.Domain.Entities.Account
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string Nome { get; set; }
        
        public string Sobrenome { get; set; }
        
        public string EmailSecundario { get; set; }
        
        public string CPF { get; set; }

        public DateTime DataCadastro { get; set; }
        
        public string Documento { get; set; }

        public DateTime DataNascimento { get; set; }
        
        public bool Genero { get; set; }
        
        public string Celular { get; set; }
        
        public bool Estudante { get; set; }
        
        public bool Bolsista { get; set; }

        public int CursosId { get; set; }

        public int InstituicaoId { get; set; }

        public long EnderecoId { get; set; }

        public bool FirstAccess { get; set; }
        
    }
}
