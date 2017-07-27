﻿using CICTED.Domain.Infrastucture.Repository.Interfaces;
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
    public class OrganizadorRepository : IOrganizadorRepository
    {
        #region
        private CustomSettings _settings;
        public OrganizadorRepository(IOptions<CustomSettings> settings)
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
                    var query = $"SELECT DATEPART(MONTH, dbo.Trabalho.DataCadastro) AS Mes, COUNT(*) As Quantidade "
                    + "FROM dbo.Trabalho "
                    + $"{(idEvento > 0 ? $"WHERE dbo.Trabalho.EventoId = {idEvento} " : "")} "
                    + "GROUP BY DATEPART(MONTH, dbo.Trabalho.DataCadastro)";

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
                    var query = $"SELECT DATEPART(MONTH, dbo.Trabalho.DataSubmissao) AS Mes, COUNT(*) As Quantidade "
                    + "FROM dbo.Trabalho "
                    + $"{(idEvento > 0 ? $"WHERE dbo.Trabalho.EventoId = {idEvento} AND" : "WHERE")} dbo.Trabalho.DataSubmissao is not null "
                    + "GROUP BY DATEPART(MONTH, dbo.Trabalho.DataSubmissao)";

                    var selectDataSubmissao = await db.QueryAsync<QuantidadeDatasViewModel>(query);
                    return selectDataSubmissao.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> GetQuantidadeTrabalhos(int idArea)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectQuantidadeTralahos = await db.QueryAsync<int>("select dbo.Trabalho.Id From dbo.SubAreaConhecimento, dbo.Trabalho Where dbo.SubAreaConhecimento.AreaConhecimentoId = 3 and dbo.Trabalho.SubAreaConhecimentoId = dbo.SubAreaConhecimento.Id ",
                        new
                        {
                            AreaConhecimentoId = idArea
                        });

                    var listaTrabalhos = selectQuantidadeTralahos.ToList();

                    return listaTrabalhos.Count();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Task<int> GetQuantidadeTrabalhosAvaliados(int idArea)
        {
            throw new NotImplementedException();
        }
        
    }
}
