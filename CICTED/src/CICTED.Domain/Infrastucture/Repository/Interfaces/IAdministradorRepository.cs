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
        Task<bool> IsAvaliador(int userID);
        Task<List<GerenciarAvaliador>> GetAvaliador();
        Task<List<GerenciarAutor>> GetAutor();
        Task<string> GetEvento(int userId);
        Task<string> GetSubAreaConhecimento(int userId);
        Task<string> GetIdentificacaoTrabalho(int userId);
    }
}
