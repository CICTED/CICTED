using CICTED.Domain.Entities.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public interface IEventoRepository
    {
        Task<List<Evento>> getEventos();
        Task<Evento> GetEvento(int IdEvento);
    }
}
