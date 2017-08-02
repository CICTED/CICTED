using CICTED.Domain.Entities.Localizacao;
using CICTED.Domain.Infrastucture.Services.Interfaces;
using CICTED.Domain.Models.Settings;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services
{
    public class LocalizacaoServices : ILocalizacaoServices
    {
        private CustomSettings _settings;

        public LocalizacaoServices(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<Estado> GetEstado(long cidadeId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var estadoId = await db.QueryAsync<int>("SELECT EstadoId FROM dbo.Cidade WHERE Id = @Id", new { Id = cidadeId });

                    var result = await db.QueryAsync<Estado>("SELECT * FROM dbo.Estado WHERE Id = @Id", new { Id = estadoId });

                    return result.FirstOrDefault();
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
