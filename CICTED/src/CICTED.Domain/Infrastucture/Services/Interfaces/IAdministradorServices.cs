using CICTED.Domain.ViewModels.Administrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services.Interfaces
{
    public interface IAdministradorServices
    {
        Task<List<Gerenciar>> GetOrganizador();
        Task<List<Gerenciar>> GetAvaliador();
        Task<string> GetEvento(long userId);
        Task<string> GetSubAreaConhecimento(long userId);
        Task<List<Gerenciar>> GetAutor();
        Task<string> GetEventoAvaliador(long userId);
        Task<string> GetAvaliadorSubArea(long userId);
    }
}
