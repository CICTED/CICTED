using CICTED.Domain.ViewModels.Administrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public interface IAdministradorRepository
    {
        Task<List<Gerenciar>> GetOrganizador();
        Task<bool> IsAvaliador(long userID);
        Task<List<Gerenciar>> GetAvaliador();
        Task<List<Gerenciar>> GetAutor();
        Task<string> GetEvento(long userId);
        Task<string> GetSubAreaConhecimento(long userId);
        Task<Gerenciar> GetOrganizador(long id);
        Task<Gerenciar> GetAvaliador(long id);
        Task<Gerenciar> GetAutor(long id);
        Task<string> GetInstituicao(long instituicaoId);
    }
}
