using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Atividades
{
    public class MinistranteAtividadeAcademica
    {
        public long MinistranteId { get; set; }

        public long AtividadeAcademicaId { get; set; }

        public Ministrantes Ministrante { get; set; }

        public AtividadeAcademica AtividadeAcademica { get; set; }
    }
}
