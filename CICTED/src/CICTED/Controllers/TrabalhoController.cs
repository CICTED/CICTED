using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.ViewModels.Autor;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.ViewModels.Trabalho;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("trabalho")]
    public class TrabalhoController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IAccountRepository _accountRepository;
        private ITrabalhoRepository _trabalhoRepository;
        private IEventoRepository _eventoRepository;

        public TrabalhoController(ITrabalhoRepository trabalhoRepository, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository, IEventoRepository eventoRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _userManager = userManager;
            _accountRepository = accountRepository;
            _eventoRepository = eventoRepository;
        }

        [HttpGet("cadastro/{IdEvento}")]
        [Authorize]
        public async Task<IActionResult> CadastroTrabalho(int IdEvento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var evento = await _eventoRepository.GetEvento(IdEvento);
            CadastroTrabalhoViewModel model = new CadastroTrabalhoViewModel()
            {
                Evento = evento.EventoNome
            };

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roles = await _accountRepository.GetRoles(user.Id);
            model.Roles = roles;
            ViewBag.Nome = user.Nome;

            return View(model);
        }

        [HttpGet("consulta")]
        [Authorize]
        public async Task<IActionResult> ConsultaTrabalho()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<long> trabalhosId = await _trabalhoRepository.GetTrabalhosId(user.Id);
            List<ConsultaTrabalho> model = new List<ConsultaTrabalho>();            

            foreach(var trabalho in trabalhosId)
            {
                model.Add(await _trabalhoRepository.ConsultaTrabalho(trabalho));
            }
                      

            return View(model);
        }

        [HttpGet("informacao")]
        [Authorize]
        public async Task<IActionResult> Informacao(long id)
        {
            var trabalho = await _trabalhoRepository.GetInformacaoTrabalho(id);
            var orientador = await _trabalhoRepository.GetOrientador(id);
            var evento = await _eventoRepository.GetEvento(trabalho.EventoId);
            var palavrasChave = await _trabalhoRepository.GetPalavrasChave(id);
            var autores = await _trabalhoRepository.GetAutores(id);
            List<AutorTrabalho> outrosAutores = new List<AutorTrabalho>() { };
            AutorTrabalho autorPrincipal = null;

            foreach(var autor in autores)
            {
                if(autor.StatusUsuarioId != 5)
                {
                    outrosAutores.Add(autor);
                }else
                {
                    autorPrincipal = autor;
                }
            }

            var model = new InformacoesTrabalhoViewModel()
            {
                Titulo = trabalho.Titulo,
                Identificacao = trabalho.Identificacao,
                Conclusao = trabalho.Conclusao,
                Metodologia = trabalho.Metodologia,
                CidadeEscola = trabalho.CidadeEscola,
                CodigoCEP = trabalho.CodigoCEP,
                DataCadastro = trabalho.DataCadastro,
                Introducao = trabalho.Introducao,
                NomeEscola = trabalho.NomeEscola,
                DataSubmissao = trabalho.DataSubmissao,
                Referencia = trabalho.Referencia,
                Resultado = trabalho.Resultado,
                TelefoneEscola = trabalho.TelefoneEscola,
                Resumo = trabalho.Resumo,
                TextoFinanciadora = trabalho.TextoFinanciadora,
                EventoNome = evento.EventoNome,
                palavrasChave = palavrasChave,
                outrosAutores = outrosAutores,
                orientador = orientador,
                autorPrincipal = autorPrincipal,                                             
            };
                  

            return Json(model);
        }

        //[HttpGet("list/subarea")]

        //public async Task<IAccountRepository> Subareas(int AreaId)

        //public async Task<IAccountRepository> Subarea(int AreaId)

        //{

        //}

    }
}
