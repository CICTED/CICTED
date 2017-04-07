using CICTED.Domain.Entities.Cursos;
using CICTED.Domain.Entities.Instituicao;
using CICTED.Domain.Entities.Localizacao;
using CICTED.Domain.Entities.Trabalho;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Account
{
    public class RegistrarViewModel
    {
        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public string CPF { get; set; }

        public string Documento { get; set; }

        public DateTime Birthday { get; set; }

        public bool Genero { get; set; }

        public string Endereco { get; set; }

        public string Bairro { get; set; }

        public string CEP { get; set; }

        //public int EstadoID { get; set; }

        //public List<Estado> Estado { get; set; }

        //public int CidadeId { get; set; }

        //public List<Cidade> Cidade { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        //public int InstituicaoId { get; set; }

        //public List<Instituicao> Instituicao { get; set; }

        public bool Estudante { get; set; }

        //public int CursoId { get; set; }

        //public List<Cursos> Curso { get; set; }

        public bool Bolsista { get; set; }

        //public int AgenciaId { get; set; }

        //public List<AgenciaFinanciadora> Agencia { get; set; }

        public string EmailPrincipal { get; set; }

        public string EmailSecundario { get; set; }

        public string ReturnMessage { get; set; }
    }
}

