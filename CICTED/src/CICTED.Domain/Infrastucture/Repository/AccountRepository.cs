
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
    public class AccountRepository : IAccountRepository
    {
        #region Construtor e Injeção

        private CustomSettings _settings;

        public AccountRepository(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }
        #endregion
    }        
}
