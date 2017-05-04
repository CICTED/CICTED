using CICTED.Domain.Infrastucture.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.Models.Settings;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using Dapper;

namespace CICTED.Domain.Infrastucture.Repository
{
    public class TrabalhoRepository : ITrabalhoRepository
    {
        #region Construtor e injeções
        private CustomSettings _settings;

        public TrabalhoRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }


        #endregion
        public async Task<Evento> GetEvento(int IdEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectEventoQuery = await db.QueryAsync<Evento>("SELECT * FROM dbo.Evento WHERE Id = @Id", 
                        new {
                            Id = IdEvento
                        });

                    return selectEventoQuery.FirstOrDefault();
                }

            }catch(Exception ex)
            {
                return null;
            }

        }
    }
}
