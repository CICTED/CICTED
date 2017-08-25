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

        public async Task<long> InsertAvaliadorSubArea(int idSubArea, long idUser)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var avaliadorSubAreaInsertQuery = "INSERT INTO dbo.AvaliadorSubAreaConhecimento(UsuarioId, SubAreaConhecimentoId) VALUES (@UsuarioId, @SubAreaConhecimentoId)";

                    var avaliadorSubAreaInsert = await db.QueryAsync<long>(avaliadorSubAreaInsertQuery,
                        new
                        {
                            UsuarioId = idUser,
                            SubAreaConhecimentoId = idSubArea
                        });

                    return avaliadorSubAreaInsert.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<long> InsertAvaliadorEvento(int idEvento, long idUser)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var avaliadorEventoInsertQuery = "INSERT INTO dbo.AvaliadorEvento(EventoId, UsuarioId) VALUES (@EventoId, @UsuarioId)";

                    var avaliadorEventoInsert = await db.QueryAsync<long>(avaliadorEventoInsertQuery,
                        new
                        {
                            EventoId = idEvento,
                            UsuarioId = idUser
                        });
                    return avaliadorEventoInsert.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<bool> Excluir(string id)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var result = await db.ExecuteAsync("DELETE * FROM dbo.AspNetUsers WHERE Id = @ID",
                        new
                        {
                            ID = id
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
