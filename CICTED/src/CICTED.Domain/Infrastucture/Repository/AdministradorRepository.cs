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
                    var organizador = selectOrganizadoresId.FirstOrDefault();

                    var selectOrganizadores = await db.QueryAsync<GerenciarOrganizador>("SELECT * FROM dbo.AspNetUsers WHERE Id = @organizadorId", new { organizadorId = organizador });

                    return selectOrganizadores.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
