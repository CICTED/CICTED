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
        Task<List<QuantidadeDatasViewModel>> GetQuantidadeDatasCadastrados(int idEvento = 0);
        Task<List<QuantidadeDatasViewModel>> GetQuantidadeDatasSubmetidos(int idEvento = 0);
        Task<int> GetQuantidadeTrabalhos(int idArea, int idEvento = 0);        
        Task<List<Gerenciar>> GetUsuarios();
        Task<Gerenciar> GetUsuarios(long id);
    }
}
