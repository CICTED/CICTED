using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.Models.Settings;
using CICTED.Domain.ViewModels.Account;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public class AutorRepository : IAutorRepository
    {
        #region Construtor e injeções
        private CustomSettings _settings;

        public AutorRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }


        #endregion
        public async Task<List<AutorTrabalho>> GetAutores(long id)
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

        public async Task<AutorViewModel> GetAutor(long userId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var coautor = await db.QueryAsync<AutorViewModel>("SELECT Nome, Sobrenome, Email FROM dbo.AspNetUsers Where Id = @Id", new { Id = userId });

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
                    var status = await db.QueryAsync<int>("SELECT StatusUsuarioId FROM dbo.AutorTrabalho Where UsuarioId = @UsuarioId", new { UsuarioId = userId });

                    return status.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<AutorTrabalho>> GetAutoresId(long id)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectTrabalhoQuery = await db.QueryAsync<AutorTrabalho>("SELECT StatusUsuarioId, UsuarioId, Orientador, AutorResponsavel FROM dbo.AutorTrabalho WHERE TrabalhoId = @TrabalhoId", new { TrabalhoId = id });

                    return selectTrabalhoQuery.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<AutorViewModel>> PesquisaAutor(string busca)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {

                    var separa = busca.Split(' ');
                    var nome = separa[0];
                    var sobrenome = nome;
                    var conta = 0;
                    foreach (var palavra in separa)
                    {
                        conta += 1;
                    }
                    if (conta > 1)
                    {
                        sobrenome = separa[1];
                    }

                    var queryBusca = await db.QueryAsync<AutorViewModel>("SELECT Id, Nome, Sobrenome, Email, InstituicaoId FROM dbo.AspNetUsers WHERE dbo.AspNetUsers.Nome LIKE '%' + @nome + '%' OR Sobrenome LIKE '%' + @sobrenome + '%'", new { nome = nome, sobrenome = sobrenome });


                    return queryBusca.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AutorTrabalho> selectOrientador(long idTrabalho)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectOrientador = await db.QueryAsync<AutorTrabalho>("SELECT * FORM dbo.AutorTrabalho WHERE IdTrabalho = @IdTrabalho AND Orientador = @Orientador",
                        new
                        {
                            IdTrabalho = idTrabalho,
                            Orientador = 1,
                        });
                    return selectOrientador.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AutorTrabalho> selectAutores(long idTrabalho)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectOrientador = await db.QueryAsync<AutorTrabalho>("SELECT * FORM dbo.AutorTrabalho WHERE IdTrabalho = @IdTrabalho",
                        new
                        {
                            IdTrabalho = idTrabalho,
                        });
                    return selectOrientador.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<bool> InsertAutorTrabalho(long idTrabalho, long idUsuario, int statusAutor, bool orientador)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var inserAutorTrabalho = await db.ExecuteAsync("INSERT INTO dbo.AutorTrabaho(StatusUsuarioId, UsuarioId, TrabalhoId, Orientador) VALUES (@StatusUsuarioId, @UsuarioId, @TrabalhoId, @Orientador)",
                        new
                        {
                            StatusUsuarioId = statusAutor,
                            UsuarioId = idUsuario,
                            TrabalhoId = idTrabalho,
                            Orientador = orientador
                        });

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
