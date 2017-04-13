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
using Microsoft.Extensions.Logging;
<<<<<<< HEAD
using CICTED.Domain.Repository.Interfaces;
=======
using MimeKit;
using MailKit.Net.Smtp;
using CICTED.Domain.Infrastucture.Services.Interfaces;
>>>>>>> caf3e4a063541e23e20bb012874f5edf8815f57c

namespace CICTED.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private IEmailServices _emailServices;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private ILocalizacaoRepository _localizacaoRepository;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailServices emailServices)
        {
            _emailServices = emailServices;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = " ")
        {
            LoginViewModel model = new LoginViewModel { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = await _signInManager.PasswordSignInAsync(model.EmailLogin,
                   model.SenhaLogin, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }




        }

        [HttpPost("cadastro")]
        public async Task<IActionResult> Cadastrar(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (model.SenhaCadastro != model.ConfirmSenhaCadastro)
                {
                    ViewBag.ErroSenha = "Senha não correspondente";
                    return View("Login", model);
                }

                var user = new ApplicationUser
                {
                    Email = model.EmailCadastro,
                    NormalizedEmail = model.EmailCadastro.ToUpper(),
                    UserName = model.EmailCadastro,
                    NormalizedUserName = model.EmailCadastro.ToUpper(),
                    DataCadastro = DateTime.Now,
                    CursosId = 1,
                    InstituicaoId = 1,
                    EnderecoId = 1
                };

                var result = await _userManager.CreateAsync(user, model.SenhaCadastro);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "AUTOR");


                    //link
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                       "ConfirmEmail", "Account",
                       new { user = user.UserName, code = code });

                    var url = $"http://localhost:54134{callbackUrl}";

                    //email
                    var email = await _emailServices.EnviarEmail(user.Email, url);                   

                    ViewBag.Cadastrado = "cadastrado";
                    return View("Login", new LoginViewModel());
                }
                else
                {
                    ViewBag.Errors = result.ConvertToHTML();
                    return View("Login", model);

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string username, string code)
        {
            try
            {
                // validacao

                var user = await _userManager.FindByNameAsync(username);
                var result = await _userManager.ConfirmEmailAsync(user, code);

                if(result.Succeeded)
                {

                }
                else
                {

                }
            }
            catch(Exception ex)
            {

            }
        }


        [HttpGet("registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar()
        {
            RegistrarViewModel model = new RegistrarViewModel();

            var estados = await _localizacaoRepository.GetEstado();
            model.Estados = estados;

            return View(model);
        }

        [HttpPost("registrar/usuario")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuario(RegistrarViewModel model)
        {
            try
            {

                var user = new ApplicationUser()
                {
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Email = model.EmailPrincipal,
                    EmailSecundario = model.EmailSecundario,
                    UserName = model.EmailPrincipal,
                    CPF = model.CPF,
                    Documento = model.Documento,
                    DataNascimento = model.DataNascimento,
                    Genero = model.Genero,
                    Celular = model.Celular,
                    Bolsista = model.Bolsista,
                    Estudante = model.Estudante                    
                };


                var result = await _userManager.CreateAsync(user, model.Senha);

                if (result.Succeeded)
                {
                    model.ReturnMessage = "Alterações salvas com sucesso";
                    return View("Login", new LoginViewModel());
                }

                else
                {
                    ViewBag.Errors = result.ConvertToHTML();
                    return View("Register", model);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
