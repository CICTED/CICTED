﻿using CICTED.Domain.Entities.Trabalho;
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
        Task<long> InsertTrabalho(int statusTrabalhoId, string titulo, string introducao, string metodologia, string resultado, string resumo, string conclusao, string referencias, string nomeEscola, string telefoneEscola, string cidadeEscola, string identificacao, DateTime dataCadastro, string textoFinanciadora, string codigoCep, int agenciaFInanciadoraId, int eventoId, long artigoId, int subAreaId, int periodoApresentacaoId);
        Task<List<PeriodoApresentacao>> GetPeriodos();        
        Task<List<string>> GetPalavrasChave(long idTrabalho);
        Task<string> GetStatusTrabalho(int statusId);
        Task<bool> GetIdentificacaoTrabalho(string identificacao);
        Task<string> GetInstituicao(long id);
        Task<List<ConsultaTrabalho>> GetTrabalhos();
        Task<bool> VerificaCadastroTrabalho(long idTrabalho, long userId);
        Task<bool> CadastraAutorTrabalho(long userId, int userStatus, bool orientador, long trabalhoId, bool autorResponsavel);
        Task<bool> DeletarAutorTrabalho(long userId, long idTrabalho);
        Task<bool> CadastrarAlunoTrabalho(long idTrabalho, List<string> nomeAluno);
        Task<Trabalho> GetTrabalho(string identificador);
        Task<SubAreaConhecimento> GetSubArea(long subAreaConhecimentoId);
        Task<bool> CadastraPalavrasChave(string palavras, long trabalhoId);
        Task<List<QuantidadeDatasViewModel>> GetQuantidadeDatasCadastrados(int idEvento = 0);
        Task<List<QuantidadeDatasViewModel>> GetQuantidadeDatasSubmetidos(int idEvento = 0);
        Task<List<QuantidadeDatasViewModel>> GetQuantidadeDataAvaliacao(int idEvento = 0);
        Task<List<long>> GetIdTtrabalhos(int idEvento);
    }
}
