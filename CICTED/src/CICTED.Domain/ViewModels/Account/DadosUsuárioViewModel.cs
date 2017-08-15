using CICTED.Domain.Entities.Cursos;
using CICTED.Domain.Entities.Instituicao;
using CICTED.Domain.Entities.Localizacao;
using CICTED.Domain.Entities.Trabalho;
using System;
using System.Collections.Generic;

namespace CICTED.Domain.ViewModels.Account
{
    public class DadosUsuárioViewModel
    {
        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public string CPF { get; set; }

        public string Documento { get; set; }

        public DateTime DataNascimento { get; set; }

        public bool Genero { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public string Complemento { get; set; }

        public string CEP { get; set; }

        public int EstadoID { get; set; }

        public int Estado { get; set; }

        public string EstadoNome { get; set; }

        public bool FirstAccess { get; set; }

        public bool EnderecoExterior { get; set; }

        public string CidadeExterior { get; set; }

        public string EstadoExterior { get; set; }

        public string Pais { get; set; }

        public List<Estado> Estados { get; set; }

        public long CidadeId { get; set; }

        public string CidadeNome { get; set; }

        public List<Cidade> Cidades { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public int InstituicaoId { get; set; }

        public List<Instituicao> Instituicoes { get; set; }

        public bool Estudante { get; set; }

        public int CursoId { get; set; }

        public string Curso { get; set; }

        public List<Cursos> Cursos { get; set; }

        public bool Bolsista { get; set; }
        
        public List<AgenciaFinanciadora> Agencia { get; set; }

        public string Email { get; set; }

        public string EmailSecundario { get; set; }

        public string Senha { get; set; }

        public string ReturnMessage { get; set; }
    }
}

