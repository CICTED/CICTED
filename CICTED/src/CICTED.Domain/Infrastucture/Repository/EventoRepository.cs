using CICTED.Domain.Infrastucture.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CICTED.Domain.Entities.Trabalho;
using System.Data.SqlClient;
using Dapper;
using CICTED.Domain.Models.Settings;
using Microsoft.Extensions.Options;

namespace CICTED.Domain.Infrastucture.Repository
{
    public class EventoRepository : IEventoRepository
    {

        #region Construtor e injecoes
        private CustomSettings _settings;

        public EventoRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }


        #endregion



        public async Task<List<Evento>> getEventos()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectEventosQuery = await db.QueryAsync<Evento>("SELECT * FROM dbo.Evento");

                    return selectEventosQuery.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<Evento> GetEvento(int IdEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectEventoQuery = await db.QueryAsync<Evento>("SELECT * FROM dbo.Evento WHERE Id = @Id",
                        new
                        {
                            Id = IdEvento
                        });

                    return selectEventoQuery.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
