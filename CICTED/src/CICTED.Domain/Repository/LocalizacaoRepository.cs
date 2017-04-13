using CICTED.Domain.Entities.Localizacao;
using CICTED.Domain.Models.Settings;
using CICTED.Domain.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Repository
{
    public class LocalizacaoRepository : ILocalizacaoRepository
    {
        private CustomSettings _settings;

        public LocalizacaoRepository(IOptions<CustomSettings> settings)
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
