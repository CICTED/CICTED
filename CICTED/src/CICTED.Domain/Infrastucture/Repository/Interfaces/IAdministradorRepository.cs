using CICTED.Domain.ViewModels.Administrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public interface IAdministradorRepository
    {
        Task<List<GerenciarOrganizador>> GetOrganizador();
        Task<bool> IsAvaliador(long userID);
        Task<List<GerenciarAvaliador>> GetAvaliador();
        Task<List<GerenciarAutor>> GetAutor();
        Task<string> GetEvento(long userId);
        Task<string> GetSubAreaConhecimento(long userId);
        Task<string> GetIdentificacaoTrabalho(long userId);
        Task<GerenciarOrganizador> GetOrganizador(long id);
        Task<GerenciarAvaliador> GetAvaliador(long id);
        Task<GerenciarAutor> GetAutor(long id);
    }
}
