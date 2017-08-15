using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.Models.Settings;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository
{
    public class AgenciaRepository : IAgenciaRepository
    {
        #region ConstrutoreInjeções
        private CustomSettings _settings;
        public AgenciaRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        #endregion
        public async Task<List<AgenciaFinanciadora>> GetAgencias()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectAgencias = await db.QueryAsync<AgenciaFinanciadora>("SELECT * FROM dbo.AgenciaFinanciadora");

                    return selectAgencias.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
