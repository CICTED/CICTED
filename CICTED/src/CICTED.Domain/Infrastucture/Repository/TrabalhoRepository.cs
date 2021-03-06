﻿using CICTED.Domain.Infrastucture.Repository.Interfaces;
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

                public async Task<long> InsertTrabalho(int statusTrabalhoId, string titulo, string introducao, string metodologia, string resultado, string resumo, string conclusao, string referencias, string nomeEscola, string telefoneEscola, string cidadeEscola, string identificacao, DateTime dataCadastro, string textoFinanciadora, string codigoCep, int agenciaFinanciadoraId, int eventoId, long artigo, int subAreaId, int periodoApresentacaoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var trabalhoInsertQuery = "INSERT INTO dbo.Trabalho(StatusTrabalhoId, Titulo, Introducao, Metodologia, Resultado, Resumo, Conclusao, Referencia, NomeEscola, TelefoneEscola, Identificacao, DataCadastro, TextoFinanciadora, CodigoCEP, AgenciaFinanciadoraId, EventoId, Artigo, SubAreaConhecimentoId, PeriodoApresentacaoId, CidadeEscola) VALUES(@StatusTrabalhoId, @Titulo, @Introducao, @Metodologia, @Resultado, @Resumo, @Conclusao, @Referencia, @NomeEscola, @TelefoneEscola, @Identiicacao,  @DataCadastro, @TextoFinanciadora, @CodigoCEP, @AgenciaFinanciadoraId, @EventoId, @Artigo, @SubAreaConhecimentoId, @PeriodoApresentacaoId, @CidadeEscola); SELECT SCOPE_IDENTITY();";

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
                            StatusTrabalhoId = statusTrabalhoId,
                            CidadeEscola = cidadeEscola
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

        public async Task<bool> GetIdentificacaoTrabalho(string identificacao)
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

        public async Task<List<ConsultaTrabalho>> GetTrabalhos()
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

        public async Task<bool> CadastraAutorTrabalho(long userId, int userStatus, bool orientador, long trabalhoId, bool autorResponsavel)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var cadastroQuery = await db.QueryAsync<bool>("INSERT INTO dbo.AutorTrabalho(StatusUsuarioId, UsuarioId, Orientador, TrabalhoId, AutorResponsavel) VALUES (@StatusUsuarioId, @UsuarioId, @Orientador, @TrabalhoId, @AutorResponsavel)",
                        new
                        {
                            UsuarioId = userId,
                            StatusUsuarioId = userStatus,
                            Orientador = orientador,
                            TrabalhoId = trabalhoId,
                            AutorResponsavel = autorResponsavel,
                        });
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CadastrarAlunoTrabalho(long idTrabalho, List<string> nomeAluno)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    foreach (var aluno in nomeAluno)
                    {
                        var result = await db.QueryAsync<bool>("INSERT INTO dbo.AlunoTrabalho(TrabalhoId, AlunoNome) VALUES (@TrabalhoId, @AlunoNome)",
                                                            new
                                                            {
                                                                TrabalhoId = idTrabalho,
                                                                AlunoNome = aluno
                                                            });
                    }
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
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var result = await db.ExecuteAsync("DELETE FROM dbo.AutorTrabalho WHERE UsuarioId = @UsuarioId AND TrabalhoId = @TrabalhoId", new { UsuarioId = userId, TrabalhoId = idTrabalho });
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Trabalho> GetTrabalho(string identificacao)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var getTrabalho = await db.QueryAsync<Trabalho>("SELECT * FROM dbo.Trabalho WHERE Identificacao = @Identificacao",
                        new
                        {
                            Identificacao = identificacao
                        });

                    return getTrabalho.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SubAreaConhecimento> GetSubArea(long subAreaConhecimentoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var getSubArea = await db.QueryAsync<SubAreaConhecimento>("SELECT * FROM dbo.SubAreaConhecimento WHERE Id = @Id",
                        new
                        {
                            Id = subAreaConhecimentoId
                        });

                    return getSubArea.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<long>> GetIdTtrabalhos(int idEvento)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var selectIdTrabalho = await db.QueryAsync<long>("SELECT Id FROM dbo.Trabalho WHERE EventoId = @EventoId",
                        new
                        {
                            EventoId = idEvento
                        });
                    return selectIdTrabalho.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeletarAutoresTrabalho(long trabalhoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = "DELETE FROM dbo.AutorTrabalho WHERE AutorResponsavel = @AutorResponsavel AND TrabalhoId = @TrabalhoId";

                    var delete = await db.ExecuteAsync(query,
                                                        new
                                                        {
                                                            AutorResponsavel = false,
                                                            TrabalhoId = trabalhoId
                                                        });

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeletarAlunosTrabalho(long trabalhoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = "DELETE FROM dbo.AlunoTrabalho WHERE TrabalhoId = @TrabalhoId";

                    var delete = await db.ExecuteAsync(query,
                                                        new
                                                        {
                                                            TrabalhoId = trabalhoId
                                                        });

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<string>> GetAlunos(long trabalhoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var query = "SELECT AlunoNome FROM dbo.AlunoTrabalho WHERE TrabalhoId = @TrabalhoId";

                    var select = await db.QueryAsync<string>(query, new { TrabalhoId = trabalhoId });

                    return select.ToList();
                }
            }catch(Exception ex)
            {
                return null;
            }
        }
    }
}