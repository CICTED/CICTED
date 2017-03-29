using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Atividades
{
    public class RecursosAtividadeAcademica
    {
        public long RecursosId { get; set; }

        public long AtividadeAcademicaId { get; set; }

        public Recursos Recursos { get; set; }

        public AtividadeAcademica AtividadeAcademica { get; set; }
    }
}
