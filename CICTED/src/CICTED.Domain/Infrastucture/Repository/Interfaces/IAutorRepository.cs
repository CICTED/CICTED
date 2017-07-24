using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.ViewModels.Account;
using CICTED.Domain.ViewModels.Trabalho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Repository.Interfaces
{
    public interface IAutorRepository
    {
        /// <summary>
        /// jnjnjhjhnjnjn
        /// </summary>
        /// <param name="id">hgbhbgh</param>
        /// <returns></returns>
        Task<List<AutorTrabalho>> GetAutores(long id);
        Task<List<string>> GetAlunos(long trabalhoId);  
        Task<int> GetStatusAutor(long userId);
        Task<List<AutorTrabalho>> GetAutoresId(long id);
        Task<AutorViewModel> GetAutor(long userId);
        Task<List<AutorViewModel>> PesquisaAutor(string busca);        
        Task<bool> InsertAutorTrabalho(long idTrabalho, long idUsuario, int statusAutor, bool orioentador);
        Task<AutorTrabalho> SelectOrientador(long idTrabalho);
    }
}
