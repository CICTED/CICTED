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

        [HttpGet("cadastro")]
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
            var user = _userManager.FindByNameAsync(User.Identity.Name);
            List<long> trabalhosId = await _trabalhoRepository.GetTrabalhosId(user.Id);
            List<Trabalho> trabalhos = new List<Trabalho>();           

            foreach(var id in trabalhosId)
            {
                trabalhos.Add(await _trabalhoRepository.GetInformacaoTrabalho(id));
            }
        
            return View();
    }

    [HttpGet("informacao")]
    [Authorize]
    public async Task<IActionResult> Informacao(long id)
    {
        var trabalho = await _trabalhoRepository.GetInformacaoTrabalho(id);
        var orientador = await _trabalhoRepository.GetOrientador(id);
        var model = new Trabalho();

        model.Identificacao = "oioi";

        return Json(model);
    }

}
}
