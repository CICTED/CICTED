using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.Models.Settings;
using CICTED.Domain.ViewModels.Administrador;
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

        public async Task<int> GetQuantidadeTrabalhosSubmetidos(int idEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = $"SELECT dbo.Trabalho.Id"
                    + " FROM dbo.Trabalho "
                    + $"{(idEvento > 0 ? $"WHERE dbo.Trabalho.EventoId = {idEvento} AND " : "WHERE ")} dbo.Trabalho.DataSubmissao is not null ";

                    var selectDataSubmissao = await db.QueryAsync<int>(query);

                    return selectDataSubmissao.ToList().Count();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        

        public async Task<int> GetQuantidadeTrabalhosAprovados(int idEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = "SELECT dbo.Trabalho.Id"
                               + " FROM dbo.SubAreaConhecimento, dbo.Trabalho "
                               + $"{(idEvento > 0 ? $"Where dbo.Trabalho.EventoId = {idEvento} AND" : "Where")} dbo.Trabalho.StatusTrabalhoId = 1";

                    var selectDataAvaliacao = await db.QueryAsync<QuantidadeDatasViewModel>(query,
                        new
                        {
                            EventoId = idEvento,
                        });

                    return selectDataAvaliacao.ToList().Count();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }


        public async Task<int> GetQuantidadeTrabalhosReprovados(int idEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = "SELECT dbo.Trabalho.Id"
                               + " FROM dbo.SubAreaConhecimento, dbo.Trabalho "
                               + $"{(idEvento > 0 ? $"Where dbo.Trabalho.EventoId = {idEvento} AND" : "Where")} dbo.Trabalho.StatusTrabalhoId = 2";

                    var selectDataAvaliacao = await db.QueryAsync<QuantidadeDatasViewModel>(query,
                        new
                        {
                            EventoId = idEvento
                        });

                    return selectDataAvaliacao.ToList().Count();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }


        public async Task<List<Gerenciar>> GetUsuarios()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectUsuarios = await db.QueryAsync<Gerenciar>("SELECT * FROM dbo.AspNetUsers");

                    return selectUsuarios.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Gerenciar> GetUsuarios(long id)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectUsuarios = await db.QueryAsync<Gerenciar>("SELECT * FROM dbo.AspNetUsers WHERE Id = @Id", new { Id = id });

                    return selectUsuarios.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
