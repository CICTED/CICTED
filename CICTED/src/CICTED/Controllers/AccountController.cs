using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.Infrastucture;
using CICTED.Domain.ViewModels.Account;
using CICTED.Domain.Infrastucture.Helpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login(string returnURL = null)
        {
            LoginViewModel model = new LoginViewModel();
            ViewData["ReturnURL"] = returnURL;

            return View();
        }

        //[HttpPost("login")]
        //public Task<IActionResult> Login()
        //{

        //}
        


        [HttpGet("cadastro")]
        [AllowAnonymous]
        public async Task<IActionResult> Cadastrar(string returnURL = null)
        {
            ViewData["ReturnURL"] = returnURL;

            return View();
        }

        [HttpPost("cadastro")]
        public async Task<IActionResult> Cadastrar(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(model.SenhaCadastro != model.ConfirmSenhaCadastro)
            {
                ModelState.AddModelError("ConfirmSenhaCadastro", "Senha não correspondente");
                return View("Cadastrar", model);
            }

            var user = new ApplicationUser
            {
                Email = model.EmailCadastro,
                NormalizedEmail = model.EmailCadastro.ToUpper(),
                UserName = model.EmailCadastro,
                NormalizedUserName = model.EmailCadastro.ToUpper(),
            };
            var result = await _userManager.CreateAsync(user, model.SenhaCadastro);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "AUTOR");
                return View("Login", new LoginViewModel());
            }
            else
            {
                ViewBag.Errors = result.ConvertToHTML();
                return View("Login", model);

            }
        }

        

    }
}
