using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.Models.Settings;
using CICTED.Domain.ViewModels.Administrador;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private CustomSettings _settings;
        public AdministradorRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<List<GerenciarOrganizador>> GetOrganizador()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectOrganizadoresId = await db.QueryAsync<long>("SELECT UserId FROM dbo.AspNetUserRoles WHERE RoleId = 2");
                    var organizador = selectOrganizadoresId.ToList();
                    List<GerenciarOrganizador> organizadores = new List<GerenciarOrganizador>();

                    foreach (var id in organizador)
                    {
                        var selectOrganizadores = await db.QueryAsync<GerenciarOrganizador>("SELECT * FROM dbo.AspNetUsers WHERE Id = @organizadorId",
                            new
                            {
                                organizadorId = id
                            });

                        GerenciarOrganizador org = selectOrganizadores.FirstOrDefault();
                        if (org != null)
                        {
                            organizadores.Add(org);
                        }


                    }

                    return organizadores;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> IsAvaliador(int userID)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectIsAvaliador = await db.QueryAsync<bool>("SELECT UserId FROM dbo.AspNetUserRoles WHERE RoleId = 3 AND UserId = @userId", new { userId = userID });

                    return selectIsAvaliador.FirstOrDefault();
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<GerenciarAvaliador>> GetAvaliador()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectAvaliadoresId = await db.QueryAsync<long>("SELECT UserId FROM dbo.AspNetUserRoles WHERE RoleId = 3");
                    var avaliador = selectAvaliadoresId.ToList();
                    List<GerenciarAvaliador> avaliadores = new List<GerenciarAvaliador>();

                    foreach (var id in avaliador)
                    {
                        var selectAvaliadores = await db.QueryAsync<GerenciarAvaliador>("SELECT * FROM dbo.AspNetUsers WHERE Id = @avaliadorId",
                            new
                            {
                                avaliadorId = id
                            });

                        GerenciarAvaliador aval = selectAvaliadores.FirstOrDefault();
                        if (aval != null)
                        {
                            avaliadores.Add(aval);
                        }
                    }

                    return avaliadores;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetEvento(int userId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var eventoId = await db.QueryAsync<int>("SELECT EventoId FROM dbo.AvaliadorEvento WHERE UsuarioId = @userId", new { userId = userId });

                    var evento = await db.QueryAsync<string>("SELECT Sigla FROM dbo.Evento WHERE Id = @eventoId", new { eventoId = eventoId });

                    return evento.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetSubAreaConhecimento(int userId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var subAreaConhecimentoId = await db.QueryAsync<int>("SELECT SubAreaConhecimentoId FROM dbo.AvaliadorSubAreaConhecimento WHERE UsuarioId = @userId", new { userId = userId });

                    var subAreaConhecimento = await db.QueryAsync<string>("SELECT Nome FROM dbo.SubAreaConhecimento WHERE Id = @subAreaConhecimentoId", new { subAreaConhecimentoId = subAreaConhecimentoId });

                    return subAreaConhecimento.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<GerenciarAutor>> GetAutor()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectAutoresId = await db.QueryAsync<long>("SELECT UserId FROM dbo.AspNetUserRoles WHERE RoleId = 4");
                    var autor = selectAutoresId.ToList();
                    List<GerenciarAutor> autores = new List<GerenciarAutor>();

                    foreach (var id in autor)
                    {
                        var selectAutores = await db.QueryAsync<GerenciarAutor>("SELECT * FROM dbo.AspNetUsers WHERE Id = @autorId",
                            new
                            {
                                autorId = id
                            });

                        GerenciarAutor aut = selectAutores.FirstOrDefault();
                        if (aut != null)
                        {
                            autores.Add(aut);
                        }
                    }

                    return autores;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetIdentificacaoTrabalho(int userId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var trabalhoId = await db.QueryAsync<int>("SELECT TrabalhoID FROM dbo.AutorTrabalho WHERE UsuarioId = @userId", new { userId = userId });

                    var identificação = await db.QueryAsync<string>("SELECT Identificacao FROM dbo.Trabalho WHERE Id = @trabalhoId", new { trabalhoId = trabalhoId });

                    return identificação.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
