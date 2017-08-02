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
    public class AreaServices : IAreaServices
    {
        private CustomSettings _settings;
        public AreaServices(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<string> GetArea(int subAreaId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var getAreaIdQuery = await db.QueryAsync<int>("SELECT AreaConhecimentoId FROM dbo.SubAreaConhecimento where Id = @Id", new { Id = subAreaId });

                    var areaId = getAreaIdQuery.FirstOrDefault();

                    var getAreaId = await db.QueryAsync<string>("SELECT Area from dbo.AreaConhecimento where Id = @Id", new { Id = areaId });

                    return getAreaId.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
