﻿using CICTED.Domain.Infrastucture.Repository.Interfaces;
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
        public async Task<List<Gerenciar>> GetOrganizador()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectOrganizadoresId = await db.QueryAsync<long>("SELECT UserId FROM dbo.AspNetUserRoles WHERE RoleId = 2");
                    var organizador = selectOrganizadoresId.ToList();
                    List<Gerenciar> organizadores = new List<Gerenciar>();

                    foreach (var id in organizador)
                    {
                        var selectOrganizadores = await db.QueryAsync<Gerenciar>("SELECT * FROM dbo.AspNetUsers WHERE Id = @organizadorId",
                            new
                            {
                                organizadorId = id
                            });

                        Gerenciar org = selectOrganizadores.FirstOrDefault();
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

        public async Task<bool> IsAvaliador(long userID)
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

        public async Task<bool> IsOrganizador(long userID)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectIsOrganizador = await db.QueryAsync<bool>("SELECT UserId FROM dbo.AspNetUserRoles WHERE RoleId = 2 AND UserId = @userId", new { userId = userID });

                    return selectIsOrganizador.FirstOrDefault();
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsAutor(long userID)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectIsAutor = await db.QueryAsync<bool>("SELECT UserId FROM dbo.AspNetUserRoles WHERE RoleId = 4 AND UserId = @userId", new { userId = userID });

                    return selectIsAutor.FirstOrDefault();
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsAdministrador(long userID)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectIsAdministrador = await db.QueryAsync<bool>("SELECT UserId FROM dbo.AspNetUserRoles WHERE RoleId = 1 AND UserId = @userId", new { userId = userID });

                    return selectIsAdministrador.FirstOrDefault();
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Gerenciar>> GetAvaliador()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectAvaliadoresId = await db.QueryAsync<long>("SELECT UserId FROM dbo.AspNetUserRoles WHERE RoleId = 3");
                    var avaliador = selectAvaliadoresId.ToList();
                    List<Gerenciar> avaliadores = new List<Gerenciar>();

                    foreach (var id in avaliador)
                    {
                        var selectAvaliadores = await db.QueryAsync<Gerenciar>("SELECT * FROM dbo.AspNetUsers WHERE Id = @avaliadorId",
                            new
                            {
                                avaliadorId = id
                            });

                        Gerenciar aval = selectAvaliadores.FirstOrDefault();
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

        public async Task<string> GetEvento(long userId)
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

        public async Task<string> GetSubAreaConhecimento(long userId)
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

        public async Task<List<Gerenciar>> GetAutor()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectAutoresId = await db.QueryAsync<long>("SELECT UserId FROM dbo.AspNetUserRoles WHERE RoleId = 4");
                    var autor = selectAutoresId.ToList();
                    List<Gerenciar> autores = new List<Gerenciar>();

                    foreach (var id in autor)
                    {
                        var selectAutores = await db.QueryAsync<Gerenciar>("SELECT * FROM dbo.AspNetUsers WHERE Id = @autorId",
                            new
                            {
                                autorId = id
                            });

                        Gerenciar aut = selectAutores.FirstOrDefault();
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

        public async Task<Gerenciar> GetOrganizador(long id)
        {
            using (var db = new SqlConnection(_settings.ConnectionString))
            {
                var selectOrganizadores = await db.QueryAsync<Gerenciar>("SELECT * FROM dbo.AspNetUsers WHERE Id = @organizadorId",
                            new
                            {
                                organizadorId = id
                            });

                return selectOrganizadores.FirstOrDefault();
            }
        }

        public async Task<Gerenciar> GetAvaliador(long id)
        {
            using (var db = new SqlConnection(_settings.ConnectionString))
            {
                var selectAvaliadores = await db.QueryAsync<Gerenciar>("SELECT * FROM dbo.AspNetUsers WHERE Id = @avaliadorId",
                            new
                            {
                                avaliadorId = id
                            });

                return selectAvaliadores.FirstOrDefault();
            }
        }

        public async Task<Gerenciar> GetAutor(long id)
        {
            using (var db = new SqlConnection(_settings.ConnectionString))
            {
                var selectAutores = await db.QueryAsync<Gerenciar>("SELECT * FROM dbo.AspNetUsers WHERE Id = @autorId",
                           new
                           {
                               autorId = id
                           });

                return selectAutores.FirstOrDefault();
            }
        }

        public async Task<string> GetInstituicao(long instituicaoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var instituicao = await db.QueryAsync<string>("SELECT InstituicaoNome FROM dbo.Instituicao WHERE Id = @instituicaoId", new { instituicaoId = instituicaoId });

                    
                    return instituicao.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
