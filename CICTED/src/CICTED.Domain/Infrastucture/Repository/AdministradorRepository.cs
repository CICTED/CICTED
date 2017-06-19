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
                    var isAvaliador = selectIsAvaliador.FirstOrDefault();

                    return isAvaliador;
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
