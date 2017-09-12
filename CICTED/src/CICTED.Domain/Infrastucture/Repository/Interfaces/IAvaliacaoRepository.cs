using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository
{
   public interface IAvaliacaoRepository
    {
        Task<bool> InsertAvaliacao(int statusTrabalhoId, long usuarioId, long trabalhoId, float nota, float notaResumo, float notaMetodologia, float notaResultado, float notaObjetivo, bool favorito, string comentario, DateTime dataAvaliacao, int tipoAvaliacao); 

    }
}
