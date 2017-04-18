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

        public async Task<List<Estado>> GetEstado()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var result = await db.QueryAsync<Estado>("SELECT * FROM dbo.Estado");

                    return result.ToList();
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Cidade>> GetCidade(int estadoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var result = await db.QueryAsync<Cidade>("SELECT * FROM dbo.Cidade WHERE EstadoId = @ESTADO", new { ESTADO = estadoId });

                    return result.ToList();
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
