using System;
using System.Collections.Generic;
using System.Text;

namespace CICTED.Domain.ViewModels.Avaliador
{
    public class AvaliadorHomeViewModel
    {
        public int Pendentes { get; set; }
        public int Avaliados { get; set; }
        public int Relevantes { get; set; }
        public int Aprovados { get; set; }
        public int Reprovados { get; set; }
    }
}
