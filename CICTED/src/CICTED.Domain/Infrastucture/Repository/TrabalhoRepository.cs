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
using CICTED.Domain.ViewModels.Trabalho;
using CICTED.Domain.ViewModels.Account;

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

        public async Task<Trabalho> GetInformacaoTrabalho(long id)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectTrabalhoQuery = await db.QueryAsync<Trabalho>("SELECT * FROM dbo.Trabalho WHERE Id = @Id", new { Id = id });

                    return selectTrabalhoQuery.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<List<long>> GetTrabalhosId(long userId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectTrabalhoQuery = await db.QueryAsync<long>("SELECT TrabalhoId FROM dbo.AutorTrabalho WHERE UsuarioId = @UsuarioId", new { UsuarioId = userId });

                    return selectTrabalhoQuery.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ConsultaTrabalho> ConsultaTrabalho(long idTrabalho)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectQuery = await db.QueryAsync<ConsultaTrabalho>("SELECT Id, Identificacao, DataCadastro, Titulo, StatusTrabalhoId FROM dbo.Trabalho WHERE Id = @Id", new { Id = idTrabalho });

                    return selectQuery.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

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



        public async Task<long> InsertTrabalho(int statusTrabalhoId, string titulo, string introducao, string metodologia, string resultado, string resumo, string conclusao, string referencias, string nomeEscola, string telefoneEscola, string cidadeEscola, string identificacao, DateTime dataCadastro, string textoFinanciadora, string codigoCep, int agenciaFinanciadoraId, int eventoId, long artigo, int subAreaId, int periodoApresentacaoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var trabalhoInsertQuery = "INSERT INTO dbo.Trabalho(StatusTrabalhoId, Titulo, Introducao, Metodologia, Resultado, Resumo, Conclusao, Referencia, NomeEscola, TelefoneEscola, Identificacao, DataCadastro, TextoFinanciadora, CodigoCEP, AgenciaFinanciadoraId, EventoId, Artigo, SubAreaConhecimentoId, PeriodoApresentacaoId) VALUES(@StatusTrabalhoId, @Titulo, @Introducao, @Metodologia, @Resultado, @Resumo, @Conclusao, @Referencia, @NomeEscola, @TelefoneEscola, @Identiicacao,  @DataCadastro, @TextoFinanciadora, @CodigoCEP, @AgenciaFinanciadoraId, @EventoId, @Artigo, @SubAreaConhecimentoId, @PeriodoApresentacaoId); SELECT SCOPE_IDENTITY();";

                    var trabalhoInsert = await db.QueryAsync<long>(trabalhoInsertQuery,
                        new
                        {
                            Titulo = titulo,
                            Introducao = introducao,
                            Metodologia = metodologia,
                            Resultado = resultado,
                            Resumo = resumo,
                            Conclusao = conclusao,
                            Referencia = referencias,
                            NomeEscola = nomeEscola,
                            TelefoneEscola = telefoneEscola,
                            Identiicacao = identificacao,
                            DataCadastro = dataCadastro,
                            TextoFinanciadora = textoFinanciadora,
                            CodigoCEP = codigoCep,
                            AgenciaFinanciadoraId = agenciaFinanciadoraId,
                            EventoId = eventoId,
                            Artigo = artigo,
                            SubAreaConhecimentoId = subAreaId,
                            PeriodoApresentacaoId = periodoApresentacaoId,
                            StatusTrabalhoId = statusTrabalhoId
                        });
                    return trabalhoInsert.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<PeriodoApresentacao>> GetPeriodos()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var getPeriodosQuery = await db.QueryAsync<PeriodoApresentacao>("SELECT * FROM dbo.PeriodoApresentacao");

                    return getPeriodosQuery.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

       

        public async Task<string> GetStatusTrabalho(int statusId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var status = await db.QueryAsync<string>("SELECT StatusTrabalhoNome FROM dbo.StatusTrabalho where Id = @Id", new { Id = statusId });

                    return status.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> getIdentificacaoTrabalho(string identificacao)
        {
            try
            {
                using (var bd = new SqlConnection(_settings.ConnectionString))
                {
                    var selectIdentifiTrabalho = await bd.QueryAsync<string>("Select Identificacao FROM dbo.Trabalho WHERE Identificacao = @Identificacao",
                        new
                        {
                            Identificacao = identificacao
                        });
                    if (selectIdentifiTrabalho.FirstOrDefault() == null)
                    {
                        return true;
                    }
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> GetInstituicao(long instituicaoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var instituicao = await db.QueryAsync<string>("SELECT InstituicaoNome FROM dbo.Instituicao WHERE Id = @Id", new { Id = instituicaoId });
                    return instituicao.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ConsultaTrabalho>> GetTrabalho()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectTrabalhoQuery = await db.QueryAsync<ConsultaTrabalho>("SELECT * FROM dbo.Trabalho");

                    return selectTrabalhoQuery.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> VerificaCadastroTrabalho(long idTrabalho, long userId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var verificaQuery = await db.QueryAsync<int>("SELECT StatusUsuarioId FROM dbo.AutorTrabalho WHERE UsuarioId = @UsuarioId AND TrabalhoId = @TrabalhoId", new { UsuarioId = userId, TrabalhoId = idTrabalho });
                    var verifica = verificaQuery.FirstOrDefault();

                    if (verifica != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CadastraAutorTrabalho(long userId, int userStatus, bool orientador, long trabalhoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var cadastroQuery = await db.QueryAsync<bool>("INSERT INTO dbo.AutorTrabalho(StatusUsuarioId, UsuarioId, Orientador, TrabalhoId) VALUES (@StatusUsuarioId, @UsuarioId, @Orientador, @TrabalhoId)",
                        new
                        {
                            UsuarioId = userId,
                            StatusUsuarioId = userStatus,
                            Orientador = orientador,
                            TrabalhoId = trabalhoId,
                        });
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeletarAutorTrabalho(long userId, long idTrabalho)
        {
            try
            {
                using(var db = new SqlConnection(_settings.ConnectionString))
                {
                    var result = await db.ExecuteAsync("DELETE FROM dbo.AutorTrabalho WHERE UsuarioId = @UsuarioId AND TrabalhoId = @TrabalhoId", new { UsuarioId = userId, TrabalhoId = idTrabalho });
                }
                return true;

            }catch(Exception ex)
            {
                return false;
            }
        }

    }
}
