using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.Models.Settings;
using CICTED.Domain.ViewModels.Trabalho;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        #region
        private CustomSettings _settings;
        public DashboardRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        #endregion

        public async Task<List<QuantidadeDatasViewModel>> GetQuantidadeDatasCadastrados(int idEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = $"SELECT dbo.Trabalho.DataCadastro, DATEPART(MONTH, dbo.Trabalho.DataCadastro) AS Mes, COUNT(*) As Quantidade "
                    + "FROM dbo.Trabalho "
                    + $"{(idEvento > 0 ? $"WHERE dbo.Trabalho.EventoId = {idEvento} " : "")} "
                    + "GROUP BY DATEPART(MONTH, dbo.Trabalho.DataCadastro), dbo.Trabalho.DataCadastro";

                    var selectDataCadastrados = await db.QueryAsync<QuantidadeDatasViewModel>(query);
                    return selectDataCadastrados.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<QuantidadeDatasViewModel>> GetQuantidadeDatasSubmetidos(int idEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = $"SELECT dbo.Trabalho.DataSubmissao, DATEPART(MONTH, dbo.Trabalho.DataSubmissao) AS Mes, COUNT(*) As Quantidade "
                    + "FROM dbo.Trabalho "

                    + $"{(idEvento > 0 ? $"WHERE dbo.Trabalho.EventoId = {idEvento} AND" : "WHERE")} dbo.Trabalho.DataSubmissao is not null "
                    + "GROUP BY DATEPART(MONTH, dbo.Trabalho.DataSubmissao), dbo.Trabalho.DataSubmissao";

                    var selectDataSubmissao = await db.QueryAsync<QuantidadeDatasViewModel>(query);
                    return selectDataSubmissao.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<QuantidadeDatasViewModel>> GetQuantidadeDataAvaliacao(int idEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = "SELECT dbo.AvaliacaoTrabalho.DataAvaliacao, DATEPART(MONTH, dbo.Trabalho.DataCadastro) AS Mes, COUNT(*) As Quantidade " +
                                "FROM dbo.Trabalho, dbo.AvaliacaoTrabalho " +
                                "WHERE dbo.AvaliacaoTrabalho.TrabalhoId = dbo.Trabalho.Id " +
                                "GROUP BY DATEPART(MONTH, dbo.Trabalho.DataCadastro), dbo.AvaliacaoTrabalho.DataAvaliacao";
                    var selectDataAvaliacao = await db.QueryAsync<QuantidadeDatasViewModel>(query);
                    return selectDataAvaliacao.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
