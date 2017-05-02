using CICTED.Domain.Entities.Cursos;
using CICTED.Domain.Entities.Instituicao;
using CICTED.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> UpdateDadosUsuario(RegistrarViewModel user, long enderecoId, long id);
        Task<List<Instituicao>> GetInstituicao();
        Task<List<Cursos>>GetCursos();
        Task<List<long>> GetRoles(long userId);
    }
}
