using CICTED.Domain.Infrastucture.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.Models.Settings;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using Dapper;
using CICTED.Domain.ViewModels.Trabalho;
using CICTED.Domain.ViewModels.Account;

namespace CICTED.Domain.Infrastucture.Repository
{
    public class TrabalhoRepository : ITrabalhoRepository
    {
        #region Construtor e injeções
        private CustomSettings _settings;

        public TrabalhoRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }


        #endregion

        public async Task<Trabalho> GetInformacaoTrabalho(long id)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectTrabalhoQuery = await db.QueryAsync<Trabalho>("SELECT * FROM dbo.Trabalho WHERE Id = @Id", new { Id = id });

                    return selectTrabalhoQuery.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<AutorTrabalho> GetOrientadorId(long id)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectTrabalhoQuery = await db.QueryAsync<AutorTrabalho>("SELECT * FROM dbo.AutorTrabalho WHERE TrabalhoId = @TrabalhoId AND Orientador = @Orientador", new { TrabalhoId = id, Orientador = true });

                    return selectTrabalhoQuery.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<List<long>> GetTrabalhosId(long userId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectTrabalhoQuery = await db.QueryAsync<long>("SELECT TrabalhoId FROM dbo.AutorTrabalho WHERE UsuarioId = @UsuarioId", new { UsuarioId = userId });

                    return selectTrabalhoQuery.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ConsultaTrabalho> ConsultaTrabalho(long idTrabalho)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectQuery = await db.QueryAsync<ConsultaTrabalho>("SELECT Id, Identificacao, DataCadastro, Titulo, StatusTrabalhoId FROM dbo.Trabalho WHERE Id = @Id", new { Id = idTrabalho });

                    return selectQuery.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<string>> GetPalavrasChave(long idTrabalho)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var queryPalavraId = await db.QueryAsync<long>("SELECT PalavraChaveId from dbo.PalavraChaveTrabalho where TrabalhoId = @TrabalhoId", new { TrabalhoId = idTrabalho });

                    var palavraId = queryPalavraId.ToList();

                    List<string> palavras = new List<string>() { };

                    foreach (var palavra in palavraId)
                    {
                        var queryPalavra = await db.QueryAsync<string>("SELECT Palavra from dbo.PalavraChave where Id = @Id", new { Id = palavra });

                        palavras.Add(queryPalavra.FirstOrDefault());
                    }
                    return palavras;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<AutorTrabalho>> GetAutoresId(long id)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectTrabalhoQuery = await db.QueryAsync<AutorTrabalho>("SELECT StatusUsuarioId, UsuarioId, Orientador FROM dbo.AutorTrabalho WHERE TrabalhoId = @TrabalhoId", new { TrabalhoId = id });

                    return selectTrabalhoQuery.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<string> GetStatusTrabalho(int statusId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var status = await db.QueryAsync<string>("SELECT StatusTrabalhoNome FROM dbo.StatusTrabalho where Id = @Id", new { Id = statusId });

                    return status.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<CoautorViewModel> GetAutor(long userId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var coautor = await db.QueryAsync<CoautorViewModel>("SELECT Nome, Sobrenome, Email FROM dbo.AspNetUsers Where Id = @Id", new { Id = userId });

                    return coautor.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> GetStatusAutor(long userId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var coautor = await db.QueryAsync<int>("SELECT StatusUsuarioId FROM dbo.AutorTrabalho Where UsuarioId = @UsuarioId", new { UsuarioId = userId });

                    return coautor.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }


}
