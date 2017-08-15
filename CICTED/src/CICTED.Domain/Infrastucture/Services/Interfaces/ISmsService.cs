using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services.Interfaces
{
    public interface ISmsService
    {
        Task SendAccountConfirmation(string phone, string code);
    }
}
