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
        Task<bool> InsertTrabalho(string titulo, string introducao, string metodologia, string resultado, string resumo, string conclusao, string referencias, string nomeEscola, string telefoneEscola, string cidadeEscola, string identificacao, string dataCadastro,string textoFinanciadora, string codigoCep,int agenciaFInanciadoraId, int eventoId, long artigoId, int subAreaId);
        Task<List<PeriodoApresentacao>> GetPeriodos();
        Task<List<AgenciaFinanciadora>> GetAgencias();
    }
}
