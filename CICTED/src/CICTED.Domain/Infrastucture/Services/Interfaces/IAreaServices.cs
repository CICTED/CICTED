using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services.Interfaces
{
    public interface IAreaServices
    {
        Task<string> GetArea(int subAreaId);

    }
}
