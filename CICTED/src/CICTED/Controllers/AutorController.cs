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
        private IEventoRepository _eventoRepository;

        public AutorController(ITrabalhoRepository trabalhoRepository, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository, IEventoRepository eventoRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _userManager = userManager;
            _accountRepository = accountRepository;
            _eventoRepository = eventoRepository;
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
    }
}
