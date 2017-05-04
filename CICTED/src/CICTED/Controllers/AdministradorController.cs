using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.Infrastucture.Repository.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("administrador")]
    public class AdministradorController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IAccountRepository _accountRepository;

        public AdministradorController(UserManager<ApplicationUser> userManager, IAccountRepository accountRepository)
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
            ViewBag.Roles = roles;
            ViewBag.Nome = user.Nome;
            return View();
        }
    }

}
