using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.ViewModels.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public interface ITrabalhoRepository
    {
        Task<Trabalho> GetInformacaoTrabalho(long id);
        Task<AutorTrabalho> GetOrientador(long id);
        Task<List<long>> GetTrabalhosId(long userId);
        Task<ConsultaTrabalho> ConsultaTrabalho(long idTrabalho);
        Task<List<string>> GetPalavrasChave(long idTrabalho);
        Task<List<AutorTrabalho>> GetAutores(long id);
    }
}
