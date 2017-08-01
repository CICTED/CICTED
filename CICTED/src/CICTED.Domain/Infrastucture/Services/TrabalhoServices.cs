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
    public class TrabalhoServices
    {
        private CustomSettings _settings;

        public TrabalhoServices(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<List<string>> GetPalavrasChave(long idTrabalho)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var queryPalavraId = await db.QueryAsync<long>("SELECT PalavraChaveId from dbo.PalavraChaveTrabalho where TrabalhoId = @TrabalhoId", new { TrabalhoId = idTrabalho });

                    var palavraId = queryPalavraId.ToList();

                    List<string> palavras = new List<string>() { };

                    foreach (var palavra in palavraId)
                    {
                        var queryPalavra = await db.QueryAsync<string>("SELECT Palavra from dbo.PalavraChave where Id = @Id", new { Id = palavra });

                        palavras.Add(queryPalavra.FirstOrDefault());
                    }
                    return palavras;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<bool> CadastraPalavrasChave(string palavras, long trabalhoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var palavrasChave = palavras.Replace(" ", "");
                    var palavrasList = (palavrasChave.Split(',')).ToList();
                    var palavraId = new List<long>();

                    foreach (var palavra in palavrasList)
                    {
                        var existe = await db.QueryAsync<long>("SELECT Id FROM dbo.PalavraChave WHERE Palavra = @Palavra",
                            new
                            {
                                Palavra = palavra,
                            });

                        var result = existe.FirstOrDefault();

                        if (result != 0)
                        {
                            palavraId.Add(result);
                        }
                        else
                        {
                            var querySalvar = await db.QueryAsync<long>("INSERT INTO dbo.PalavraChave(Palavra) VALUES (@Palavra); SELECT SCOPE_IDENTITY();", new
                            {
                                Palavra = palavra,
                            });

                            palavraId.Add(querySalvar.FirstOrDefault());
                        }
                    }

                    if (palavraId != null)
                    {
                        foreach (var id in palavraId)
                        {
                            var querySalvaId = await db.ExecuteAsync("INSERT INTO dbo.PalavraChaveTrabalho(PalavraChaveId, TrabalhoId) VALUES (@PalavraChaveId, @TrabalhoId)", new
                            {
                                PalavraChaveId = id,
                                TrabalhoId = trabalhoId
                            });
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
