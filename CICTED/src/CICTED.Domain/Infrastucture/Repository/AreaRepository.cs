using CICTED.Domain.Infrastucture.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CICTED.Domain.Entities.Trabalho;
using System.Data.SqlClient;
using CICTED.Domain.Models.Settings;
using Microsoft.Extensions.Options;
using Dapper;

namespace CICTED.Domain.Infrastucture.Repository
{
    public class AreaRepository : IAreaRepository
    {
        #region Construtor e Injeção

        private CustomSettings _settings;
        public AreaRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        #endregion
        public async Task<SubAreaConhecimento> GetSubArea(int areaId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var getSubAreaQuery = await db.QueryAsync<SubAreaConhecimento>("SELECT * FROM dbo.SubAreaConhecimento WHERE  Id = @Id",
                        new
                        {
                            Id = areaId
                        });

                    return getSubAreaQuery.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
