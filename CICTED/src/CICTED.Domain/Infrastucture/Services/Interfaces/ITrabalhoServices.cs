using CICTED.Domain.ViewModels.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services.Interfaces
{
    public interface ITrabalhoServices
    {
        Task<List<string>> GetPalavrasChave(long idTrabalho);
        Task<bool> CadastraPalavrasChave(string palavras, long trabalhoId);
        Task<List<AvaliacaoTrabalhoViewModel>> TrabalhosPendentes(long idAvaliador);
    }
}
