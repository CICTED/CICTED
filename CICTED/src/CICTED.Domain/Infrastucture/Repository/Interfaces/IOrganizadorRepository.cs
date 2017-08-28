using CICTED.Domain.ViewModels.Administrador;
using CICTED.Domain.ViewModels.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
   public  interface IOrganizadorRepository
    {       
        Task<int> GetQuantidadeTrabalhosSubmetidos(int idEvento = 0);         
        Task<int> GetQuantidadeTrabalhosAprovados(int idEvento = 0);
        Task<int> GetQuantidadeTrabalhosReprovados(int idEvento = 0);


        Task<List<Gerenciar>> GetUsuarios();
        Task<Gerenciar> GetUsuarios(long id);
    }
}
