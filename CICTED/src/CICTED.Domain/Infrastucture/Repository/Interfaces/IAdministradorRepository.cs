using CICTED.Domain.ViewModels.Administrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public interface IAdministradorRepository
    {
        Task<bool> IsAvaliador(long userID);
        Task<bool> IsOrganizador(long userID);
        Task<bool> IsAutor(long userID);
        Task<bool> IsAdministrador(long userID);
        Task<Gerenciar> GetOrganizador(long id);
        Task<Gerenciar> GetAvaliador(long id);
        Task<Gerenciar> GetAutor(long id);
        Task<string> GetInstituicao(long instituicaoId);
        Task<bool> Excluir(string id);
        Task<long> InsertAvaliadorEvento(int idEvento, long idUser);
        Task<long> InsertAvaliadorSubArea(int idSubArea, long idUser);
    }
}
