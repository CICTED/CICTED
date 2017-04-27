﻿
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

        public async Task<bool> UpdateDadosUsuario(RegistrarViewModel user, long enderecoId)
        {
            try
            {
                using (var db = new SqlConnection(_settings.ConnectionString))
                {
                    var updateDadosQuery = "UPDATE dbo.AspNetUsers SET Bolsista = @Bolsista, CPF = @CPF, Celular = @Celular, " +
                        "DataNascimento = @DataNascimento, Documento = @Documento, EmailSecundario = @EmailSecundario, Estudante = @Estudante, " +
                        "Genero = @Genero, Nome = @Nome, PhoneNumber = @PhoneNumber, Sobrenome = @Sobrenome, EnderecoId = @EnderecoId, " +
                        "CursosId = @CursosId, InstituicaoId = @InstituicaoId, Email = @Email";

                    var resultadoDados = await db.ExecuteAsync(updateDadosQuery,
                                                                new
                                                                {
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
                                                                    CursosId = user.Curso,
                                                                    InstituicaoId = user.InstituicaoId,
                                                                    Email = user.Email
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
   
    }        
}
