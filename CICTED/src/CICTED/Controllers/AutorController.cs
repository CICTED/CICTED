using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.Infrastucture.Repository;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.ViewModels.Trabalho;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("autor")]
    public class AutorController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IAccountRepository _accountRepository;

        public AutorController(UserManager<ApplicationUser> userManager, IAccountRepository accountRepository)
        {
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

        [HttpGet("dados")]
        public async Task<PartialViewResult> DadosUsuario()
        {
            var model = new ApplicationUser();
            return PartialView();
        }



    }

}
