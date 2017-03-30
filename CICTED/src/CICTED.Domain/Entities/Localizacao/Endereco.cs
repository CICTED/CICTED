using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Localizacao
{
    public class Endereco
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Logradouro { get; set; }

        [Required]
        public string CEP { get; set; }

        [Required]
        public string Numero { get; set; }

        public string Complemento { get; set; }

        public long CidadeId { get; set; }

        public long EnderecoExteriorId { get; set; }
    }
}
