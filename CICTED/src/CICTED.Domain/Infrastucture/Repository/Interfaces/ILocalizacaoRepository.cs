using CICTED.Domain.Entities.Instituicao;
using CICTED.Domain.Entities.Localizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public interface ILocalizacaoRepository
    {
        Task<List<Estado>> GetEstados();
        Task<Estado> GetEstado(long cidadeId);
        Task<List<Cidade>> GetCidades(int estadoId);
        Task<Cidade> GetCidade(long cidadeId);
        Task<long> InsertEnderecoExterior(EnderecoExterior endereco);
        Task<long> InsertEndereco(Endereco endereco, long cidadeId, long enderecoExterior = 0);
    }
}
