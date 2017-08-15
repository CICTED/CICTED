using CICTED.Domain.Entities.Localizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services.Interfaces
{
    public interface ILocalizacaoServices
    {
        Task<Estado> GetEstado(long cidadeId);
    }
}
