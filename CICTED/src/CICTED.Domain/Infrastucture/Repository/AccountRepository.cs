
using CICTED.Domain.Entities.Cursos;
using CICTED.Domain.Entities.Instituicao;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.Models.Settings;
using CICTED.Domain.ViewModels.Account;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository
{
    public class AccountRepository : IAccountRepository
    {
        #region Construtor e Injeção

        private CustomSettings _settings;

        public AccountRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        #endregion

        public async Task<bool> UpdateDadosUsuario(RegistrarViewModel user, long enderecoId, long id)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var updateDadosQuery = "UPDATE dbo.AspNetUsers SET Bolsista = @Bolsista, CPF = @CPF, Celular = @Celular, " +
                        "DataNascimento = @DataNascimento, Documento = @Documento, EmailSecundario = @EmailSecundario, Estudante = @Estudante, " +
                        "Genero = @Genero, Nome = @Nome, PhoneNumber = @PhoneNumber, Sobrenome = @Sobrenome, EnderecoId = @EnderecoId, " +
                        "CursosId = @CursosId, InstituicaoId = @InstituicaoId, Email = @Email, FirstAccess = @FirstAccess WHERE Id = @Id";

                    var resultadoDados = await db.ExecuteAsync(updateDadosQuery,
                                                                new
                                                                {
                                                                    Id = id,
                                                                    Bolsista = user.Bolsista,
                                                                    CPF = user.CPF,
                                                                    Celular = user.Celular,
                                                                    DataNascimento = user.DataNascimento,
                                                                    Documento = user.Documento,
                                                                    EmailSecundario = user.EmailSecundario,
                                                                    Estudante = user.Estudante,
                                                                    Genero = user.Genero,
                                                                    Nome = user.Nome,
                                                                    PhoneNumber = user.Telefone,
                                                                    Sobrenome = user.Sobrenome,
                                                                    EnderecoId = enderecoId,
                                                                    InstituicaoId = user.InstituicaoId,
                                                                    Email = user.Email,
                                                                    CursosId = user.CursoId,
                                                                    FirstAccess = user.FirstAccess
                                                                });
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Instituicao>> GetInstituicao()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var Instituicoes = await db.QueryAsync<Instituicao>("SELECT * FROM dbo.Instituicao");

                    return Instituicoes.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Cursos>> GetCursos()
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var cursos = await db.QueryAsync<Cursos>("SELECT * FROM dbo.Cursos");

                    return cursos.ToList();
                }
            } catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<List<long>> GetRoles(long userId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var roles = await db.QueryAsync<long>("SELECT RoleId FROM dbo.AspNetUserRoles WHERE UserId = @UserId", new { UserId = userId });

                    return roles.ToList();
                }
            }catch(Exception ex)
            {
                return null;
            }
        }

    }   
}
