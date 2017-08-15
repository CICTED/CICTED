using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services.Interfaces
{
    public interface IEmailServices
    {
        Task<bool> EnviarEmail(string email, string link, string password = null);
    }
}
