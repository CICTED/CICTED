using CICTED.Domain.Entities.Instituicao;
using CICTED.Domain.Entities.Localizacao;
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
    public class LocalizacaoRepository : ILocalizacaoRepository
    {
        private CustomSettings _settings;

        public LocalizacaoRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<List<Estado>> GetEstados()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var result = await db.QueryAsync<Estado>("SELECT * FROM dbo.Estado");

                    return result.ToList();
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Estado> GetEstado(long cidadeId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var estadoId = await db.QueryAsync<int>("SELECT EstadoId FROM dbo.Cidade WHERE Id = @Id", new { Id = cidadeId });

                    var result = await db.QueryAsync<Estado>("SELECT * FROM dbo.Estado WHERE Id = @Id", new { Id = estadoId });

                    return result.FirstOrDefault();
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Cidade>> GetCidades(int estadoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var result = await db.QueryAsync<Cidade>("SELECT * FROM dbo.Cidade WHERE EstadoId = @ESTADO", new { ESTADO = estadoId });

                    return result.ToList();
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Cidade> GetCidade(long cidadeId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var result = await db.QueryAsync<Cidade>("SELECT * FROM dbo.Cidade WHERE Id = @Id", new { Id = cidadeId });

                    return result.FirstOrDefault();
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<long> InsertEnderecoExterior(EnderecoExterior endereco)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var enderecoQueryInsert = "INSERT INTO dbo.EnderecoExterior(Cidade, Estado, Pais) VALUES "
                        + "(@CIDADE, @ESTADO, @PAIS); SELECT SCOPE_IDENTITY();";

                    var enderecoInsert = await db.QueryAsync<long>(enderecoQueryInsert,
                        new
                        {
                            Cidade = endereco.Cidade,
                            Estado = endereco.Estado,
                            Pais = endereco.Pais
                        });

                    var resultEndereco = enderecoInsert.FirstOrDefault();
                    return resultEndereco;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<long> InsertEndereco(Endereco endereco, long cidadeId, long enderecoExterior = 0)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    if (enderecoExterior > 0)
                    {
                        var enderecoQuery = "INSERT INTO dbo.Endereco(Logradouro, CEP, Numero, Complemento, Bairro, EnderecoExteriorId) VALUES "
                        + "(@LOGRADOURO, @CEP, @NUMERO, @COMPLEMENTO, @BAIRRO, @ENDERECOEXTERIORID); SELECT SCOPE_IDENTITY();";

                        var enderecoInsert = await db.QueryAsync<long>(enderecoQuery,
                            new
                            {
                                Logradouro = endereco.Logradouro,
                                CEP = endereco.CEP,
                                Numero = endereco.Numero,
                                Complemento = endereco.Complemento,
                                Bairro = endereco.Bairro,
                                enderecoExteriorId = endereco.EnderecoExteriorId
                            });
                        return enderecoInsert.FirstOrDefault();
                    }
                    else
                    {
                        var enderecoQuery = "INSERT INTO dbo.Endereco(Logradouro, CEP, Numero, Complemento, Bairro, CidadeId) VALUES "
                        + "(@LOGRADOURO, @CEP, @NUMERO, @COMPLEMENTO, @BAIRRO, @CIDADEID); SELECT SCOPE_IDENTITY();";

                        var enderecoInsert = await db.QueryAsync<long>(enderecoQuery,
                            new
                            {
                                Logradouro = endereco.Logradouro,
                                CEP = endereco.CEP,
                                Numero = endereco.Numero,
                                Complemento = endereco.Complemento,
                                Bairro = endereco.Bairro,
                                CidadeId = cidadeId
                            });
                        return enderecoInsert.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<long> UpdateEnderecoExterior(EnderecoExterior endereco)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var enderecoQueryUpdate = "UPDATE dbo.EnderecoExterior SET Cidade = @CIDADE, Estado = @ESTADO, Pais = @PAIS";

                    var enderecoUpdate = await db.QueryAsync<long>(enderecoQueryUpdate,
                        new
                        {
                            Cidade = endereco.Cidade,
                            Estado = endereco.Estado,
                            Pais = endereco.Pais
                        });

                    var resultEndereco = enderecoUpdate.FirstOrDefault();
                    return resultEndereco;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<long> UpdateEndereco(Endereco endereco, long cidadeId, long enderecoId, long enderecoExterior = 0)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    if (enderecoExterior > 0)
                    {
                        var enderecoQuery = "UPDATE dbo.Endereco SET Logradouro = @LOGRADOURO, CEP = @CEP, Numero = @NUMERO, Complemento = @COMPLEMENTO, Bairro = @BAIRRO, EnderecoExteriorId = @ENDERECOEXTERIORID WHERE Id = @enderecoId";

                        var enderecoUpdate = await db.QueryAsync<long>(enderecoQuery,
                            new
                            {
                                Logradouro = endereco.Logradouro,
                                CEP = endereco.CEP,
                                Numero = endereco.Numero,
                                Complemento = endereco.Complemento,
                                Bairro = endereco.Bairro,
                                enderecoExteriorId = endereco.EnderecoExteriorId
                            });
                        return enderecoUpdate.FirstOrDefault();
                    }
                    else
                    {
                        var enderecoQuery = "UPDATE dbo.Endereco SET Logradouro = @LOGRADOURO, CEP = @CEP, Numero = @NUMERO, Complemento = @COMPLEMENTO, Bairro = @BAIRRO, CidadeId = @CIDADEID WHERE Id = @enderecoId";

                        var enderecoUpdate = await db.QueryAsync<long>(enderecoQuery,
                            new
                            {
                                Logradouro = endereco.Logradouro,
                                CEP = endereco.CEP,
                                Numero = endereco.Numero,
                                Complemento = endereco.Complemento,
                                Bairro = endereco.Bairro,
                                CidadeId = cidadeId,
                                enderecoId = enderecoId
                            });
                        return enderecoUpdate.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


    }
}

