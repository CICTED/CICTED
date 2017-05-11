﻿using CICTED.Domain.Entities.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Trabalho
{
    public class InformacoesTrabalhoViewModel
    {

        public long Id { get; set; }

        public string Titulo { get; set; }

        public string Introducao { get; set; }

        public string Metodologia { get; set; }

        public string Resultado { get; set; }

        public string Resumo { get; set; }

        public string Conclusao { get; set; }

        public string Referencia { get; set; }

        public string NomeEscola { get; set; }

        public string TelefoneEscola { get; set; }

        public string CidadeEscola { get; set; }

        public string Identificacao { get; set; }

        public DateTime DataSubmissao { get; set; }

        public DateTime DataCadastro { get; set; }

        public string TextoFinanciadora { get; set; }

        public string CodigoCEP { get; set; }

        public string EventoNome { get; set; }

        public List<string> palavrasChave { get; set; }

        public AutorTrabalho orientador { get; set; }

        public List<AutorTrabalho> autores { get; set; }

        public AutorTrabalho autorPrincipal { get; set; }
    }
}
