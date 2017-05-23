using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.ViewModels.Account;
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
        Task<List<long>> GetTrabalhosId(long userId);
        Task<ConsultaTrabalho> ConsultaTrabalho(long idTrabalho);
        Task<int> GetStatusAutor(long userId);
        Task<bool> InsertTrabalho(string titulo, string introducao, string metodologia, string resultado, string resumo, string conclusao, string referencias, string nomeEscola, string telefoneEscola, string cidadeEscola, string identificacao, DateTime dataCadastro,string textoFinanciadora, string codigoCep,int agenciaFInanciadoraId, int eventoId, long artigoId, int subAreaId, int periodoApresentacaoId);
        Task<List<PeriodoApresentacao>> GetPeriodos();
        Task<List<AgenciaFinanciadora>> GetAgencias();
        Task<List<string>> GetPalavrasChave(long idTrabalho);
        Task<List<AutorTrabalho>> GetAutoresId(long id);
        Task<string> GetStatusTrabalho(int statusId);
        Task<AutorViewModel> GetAutor(long userId);
        Task<List<AutorViewModel>> BuscaAutor(string busca);
        Task<bool> getIdentificacaoTrabalho(string identificacao);
        Task<AutorTrabalho> SelectOrientador(long idTrabalho);
        Task<AutorTrabalho> SelectAutores(long idTrabalho);
        Task<bool> InsertAutorTrabalho(long idTrabalho, long idUsuario, int statusAutor, bool orioentador);
        //Task<int> GetStatusAutor(long userId);
    }
}
