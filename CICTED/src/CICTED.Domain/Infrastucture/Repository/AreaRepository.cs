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
        public async Task<List<SubAreaConhecimento>> GetSubAreas(int areaId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var getSubAreaQuery = await db.QueryAsync<SubAreaConhecimento>("SELECT * FROM dbo.SubAreaConhecimento WHERE  AreaConhecimentoId = @AreaConhecimentoId",
                        new
                        {
                            AreaConhecimentoId = areaId
                        });

                    return getSubAreaQuery.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<List<AreaConhecimento>> GetAreas()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var getAreasQuery = await db.QueryAsync<AreaConhecimento>("SELECT * FROM dbo.AreaConhecimento");

                    return getAreasQuery.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetSubArea(int subAreaId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var getSubAreaQuery = await db.QueryAsync<string>("SELECT Nome FROM dbo.SubAreaConhecimento where Id = @Id", new { Id = subAreaId });

                    return getSubAreaQuery.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //public async Task<List<SubAreaConhecimento>> GetSubAreass(List<int> areaId)
        //{
        //    try
        //    {
        //        using (var db = new SqlConnection(_settings.ConnectionString))
        //        {
        //            var getSubAreaQuery = await db.QueryAsync<SubAreaConhecimento>("SELECT * FROM dbo.SubAreaConhecimento WHERE  AreaConhecimentoId = @AreaConhecimentoId",
        //                new
        //                {
        //                    AreaConhecimentoId = areaId
        //                });

        //            return getSubAreaQuery.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

    }
}
