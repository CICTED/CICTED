using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.ViewModels.Trabalho
{
    public class DashboardViewModel
    {
        public int Cadastrados { get; set; }
        public int Submetidos { get; set; }
        public int Avaliados { get; set; }
        public int Aprovados { get; set; }
        public int Reprovados { get; set; }
        public List<int> CadastradosMes { get; set; } = new List<int>();
        public List<int> AvaliadosMes { get; set; } = new List<int>();
        public List<int> SubmetidosMes { get; set; } = new List<int>();

    }
}
