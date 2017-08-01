using CICTED.Domain.Models.Settings;
using CICTED.Domain.ViewModels.Trabalho;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services.Interfaces
{
    public class OrganizadorServices : IOrganizadorServices
    {

        #region
        private CustomSettings _settings;
        public OrganizadorServices(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        #endregion
        public async Task<List<QuantidadeDatasViewModel>> GetQuantidadeDataAvaliacao(int idEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = "SELECT DATEPART(MONTH, dbo.Trabalho.DataCadastro) AS Mes, COUNT(*) As Quantidade " +
                                "FROM dbo.Trabalho, dbo.AvaliacaoTrabalho " +
                                "WHERE dbo.AvaliacaoTrabalho.TrabalhoId = dbo.Trabalho.Id AND dbo.Trabalho.EventoId = @EventoId " +
                                "GROUP BY DATEPART(MONTH, dbo.Trabalho.DataCadastro)";
                    var selectDataAvaliacao = await db.QueryAsync<QuantidadeDatasViewModel>(query,new {EventoId = idEvento });
                    return selectDataAvaliacao.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> GetQuantidadeTrabalhosAvaliados(int idArea, int idEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = "SELECT dbo.Trabalho.Id FROM dbo.Trabalho, dbo.AvaliacaoTrabalho "+
                                 "WHERE dbo.AvaliacaoTrabalho.TrabalhoId = dbo.Trabalho.Id and dbo.Trabalho.EventoId = @EventoId";
                    var selectDataAvaliacao = await db.QueryAsync<QuantidadeDatasViewModel>(query,new { EventoId = idEvento });

                    return selectDataAvaliacao.ToList().Count();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
    }
}
