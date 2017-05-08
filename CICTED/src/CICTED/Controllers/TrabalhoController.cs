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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("trabalho")]
    public class TrabalhoController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IAccountRepository _accountRepository;
        private ITrabalhoRepository _trabalhoRepository;

        public TrabalhoController(ITrabalhoRepository trabalhoRepository, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _userManager = userManager;
            _accountRepository = accountRepository;
        }

        [HttpGet("~/{id}/cadastro")]
        [Authorize]
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

        [HttpGet("consulta")]
        [Authorize]
        public async Task<IActionResult> ConsultaTrabalho()
        {
            return View();
        }

    }
}
