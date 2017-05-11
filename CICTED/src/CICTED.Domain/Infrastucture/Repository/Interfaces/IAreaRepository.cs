using CICTED.Domain.Entities.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public interface IAreaRepository
    {
        Task<List<SubAreaConhecimento>> GetSubAreas(int areaId);
        Task<List<AreaConhecimento>> GetAreas();
    }
}
