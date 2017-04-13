using CICTED.Domain.Entities.Localizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Repository.Interfaces
{
    public interface ILocalizacaoRepository
    {
        Task<List<Estado>> GetEstado();
        Task<List<Cidade>> GetCidade(int estadoId);
    }
}
