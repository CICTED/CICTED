using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.ViewModels.Autor;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("autor")]
    public class AutorController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IAccountRepository _accountRepository;
        private ITrabalhoRepository _trabalhoRepository;

        public AutorController(ITrabalhoRepository trabalhoRepository, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _userManager = userManager;
            _accountRepository = accountRepository;
        }

        [HttpGet("home")]
        [Authorize]
        public async Task<IActionResult> Home()
        {           
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roles = await _accountRepository.GetRoles(user.Id);
            ViewBag.Nome = user.Nome;
            ViewBag.Roles = roles;
            return View();
        }

        [HttpGet("~/{Idevento}/CadastroTrabalho")]
        public async Task<IActionResult> CadastroTrabalho(int Idevento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(); 
            }
            CadastroTrabalhoViewModel model = new CadastroTrabalhoViewModel();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roles = await _accountRepository.GetRoles(user.Id);           
            model.Roles = roles;
            ViewBag.Nome = user.Nome;            

            return View(model);
        }

<<<<<<< HEAD
        [HttpGet("evento")]
        public async Task<IActionResult> Evento(int id)        
        {   
=======
        [HttpGet]
        [AllowAnonymous]
        [Route("evento/{id}")]
        public async Task<IActionResult> Eventos(int id)
        {
>>>>>>> 2933273551b9cda7f89c3dfa970a6fa51fa19b5b
            var eventos = await _trabalhoRepository.GetEvento(id);

            if (eventos == null) return BadRequest("There was an error to load the event.");

            HomeViewModel model = new HomeViewModel()
            {
                EventoNome = eventos.EventoNome,
                Descricao = eventos.Descricao,
                Objetivo = eventos.Objetivo,
                PublicoAlvo = eventos.PublicoAlvo
            }; 

            return Json(eventos);
        }






    }

}
