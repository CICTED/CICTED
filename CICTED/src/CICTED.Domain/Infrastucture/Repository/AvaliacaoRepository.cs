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
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        #region Construtor e injeções
        private CustomSettings _settings;
        public AvaliacaoRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        #endregion
        public async Task<bool> InsertAvaliacao(int statusUsuarioId, long usuarioId, long trabalhoId, float nota, float notaResumo, float notaMetodologia, float notaResultado, float notaObjetivo, bool favorito, string comentario, DateTime dataAvaliacao, int tipoAvaliacao)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var insetAvaliacao = await db.QueryAsync<bool>("INSERT INTO dbo.Avaliacao(StatusUsuarioId, UsuarioId, TrabalhoId, Nota, NotaResumo, NotaMetodologia, NotaResultado, NotaObjetivo, Favorito, Comentario, DataAvaliacao, TipoAvaliacao) VALUES(@StatusUsuarioId, @UsuarioId, @TrabalhoId, @Nota, @NotaResumo, @NotaMetodologia, @NotaResultado, @NotaObjetivo, @Favorito, @Comentario, @DataAvaliacao, @TipoAvaliacao)",
                        new
                        {
                            StatusUsuarioId = statusUsuarioId,
                            UsuarioId = usuarioId,
                            TrabalhoId = trabalhoId,
                            Nota = nota,
                            NotaResumo = notaResumo,
                            NotaMetodologia = notaMetodologia,
                            NotaResultado = notaResultado,
                            NotaObjetivo = notaObjetivo,
                            Favorito = favorito,
                            Comentario = comentario,
                            DataAvaliacao = dataAvaliacao,
                            TipoAvaliacao = tipoAvaliacao,
                        });
                
                    return true;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
            
        }
    }
}
