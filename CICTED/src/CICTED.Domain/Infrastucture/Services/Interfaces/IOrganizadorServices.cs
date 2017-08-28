using CICTED.Domain.ViewModels.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services.Interfaces
{
    public interface IOrganizadorServices
    {
            
        Task<int> GetQuantidadeTrabalhosArea(int idArea, int idEvento = 0);
        Task<int> GetQuantidadeTrabalhosAvaliadosArea(int idArea, int idEvento = 0);    
    }
}
