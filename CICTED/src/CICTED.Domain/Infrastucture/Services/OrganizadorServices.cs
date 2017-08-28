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
       

        public async Task<int> GetQuantidadeTrabalhosArea(int idArea, int idEvento = 0)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectQuantidadeTrabalhos = await db.QueryAsync<int>("SELECT dbo.Trabalho.Id"
                                                          + " FROM dbo.SubAreaConhecimento, dbo.Trabalho "
                                                          + $"{(idEvento > 0 ? $"Where dbo.Trabalho.EventoId = {idEvento} AND" : "Where")} dbo.SubAreaConhecimento.AreaConhecimentoId = @AreaConhecimentoId AND dbo.Trabalho.SubAreaConhecimentoId = dbo.SubAreaConhecimento.Id",
                        new
                        {
                            AreaConhecimentoId = idArea,
                        });

                    var listaTrabalhos = selectQuantidadeTrabalhos.ToList();

                    return listaTrabalhos.Count();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetQuantidadeTrabalhosAvaliadosArea(int idArea, int idEvento = 0)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectTrabalhosAvaliados = await db.QueryAsync<int>("SELECT dbo.Trabalho.Id"
                                                          + " FROM dbo.SubAreaConhecimento, dbo.Trabalho, dbo.AvaliacaoTrabalho "
                                                          + $"{(idEvento > 0 ? $"Where dbo.Trabalho.EventoId = {idEvento} AND" : "Where")} dbo.SubAreaConhecimento.AreaConhecimentoId = @AreaConhecimentoId" 
                                                          + " AND dbo.Trabalho.SubAreaConhecimentoId = dbo.SubAreaConhecimento.Id and dbo.AvaliacaoTrabalho.TrabalhoId = dbo.Trabalho.Id AND dbo.AvaliacaoTrabalho.TipoAvaliacao = 1",
                                                          new
                                                          {
                                                              AreaConhecimentoId = idArea
                                                          });
                    return selectTrabalhosAvaliados.ToList().Count();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
