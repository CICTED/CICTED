using CICTED.Domain.ViewModels.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
   public  interface IDashboardRepository
    {
        Task<List<QuantidadeDatasViewModel>> GetQuantidadeDatasCadastrados(int idEvento = 0);
        Task<List<QuantidadeDatasViewModel>> GetQuantidadeDatasSubmetidos(int idEvento = 0);
        Task<List<QuantidadeDatasViewModel>> GetQuantidadeDataAvaliacao(int idEvento = 0);
    }
}
