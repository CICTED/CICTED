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
    }
}
