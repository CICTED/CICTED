using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Entities.Atividades
{
    public class PublicoAtividade
    {
        public long PublicoAlvoId { get; set; }

        public long AtividadeAcademicaId { get; set; }
    }
}
