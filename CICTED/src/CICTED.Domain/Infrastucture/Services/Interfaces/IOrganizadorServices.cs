using CICTED.Domain.ViewModels.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services.Interfaces
{
    public interface IOrganizadorServices
    {
        Task<List<QuantidadeDatasViewModel>> GetQuantidadeDataAvaliacao(int idEvento = 0);
    }
}
