using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("autor")]
    public class AutorController : Controller
    {
        private UserManager<ApplicationUser> _userManager;

        public AutorController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet("home")]
        [Authorize]
        public async Task<IActionResult> Home()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //ViewBag.Nome = user.Nome;
            return View();
        }

        [HttpGet("CadastroTrabalho")]
        public IActionResult cadastroTrabalho()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(); 
            }
            return View();
        }



    }

}
