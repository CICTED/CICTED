using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.ViewModels.Account;
using CICTED.Domain.Infrastucture.Helpers;
using CICTED.Domain.Infrastucture.Services.Interfaces;

namespace CICTED.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private IEmailServices _emailServices;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private ILocalizacaoServices _localizacaoServices;
        private ISmsService _smsService;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailServices emailServices, ILocalizacaoServices localizacaoServices, ISmsService smsService)
        {
            _localizacaoServices = localizacaoServices;
            _emailServices = emailServices;
            _signInManager = signInManager;
            _userManager = userManager;
            _smsService = smsService;
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = " ")
        {
            LoginViewModel model = new LoginViewModel { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.EmailLogin);

                if (user != null)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest();
                    }

                    if (model.ConfirmaEmail && !user.EmailConfirmed)
                    {
                        //link
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action(
                           "ConfirmEmail", "Account",
                           new { user = user.UserName, code = code });

                        var url = $"http://localhost:54134{callbackUrl}";

                        //email
                        var email = await _emailServices.EnviarEmail(user.Email, url);

                        ViewBag.EmailNaoConfirmado = true;
                        return View("Login", new LoginViewModel());
                    }

                    var result = await _signInManager.PasswordSignInAsync(model.EmailLogin,
                       model.SenhaLogin, model.RememberMe, user.EmailConfirmed);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else if (user.FirstAccess == true)
                        {
                            return RedirectToAction("Registrar");
                        }                       
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else if (model.ConfirmaEmail && user.EmailConfirmed == false)
                    {
                        //link
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action(
                           "ConfirmEmail", "Account",
                           new { user = user.UserName, code = code });

                        var url = $"http://localhost:54134{callbackUrl}";

                        //email
                        var email = await _emailServices.EnviarEmail(user.Email, url);

                        ViewBag.EmailNaoConfirmado = true;
                        return View("Login", new LoginViewModel());
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt");
                        return View(model);
                    }
                }
                ViewBag.UsuarioNaoExiste = true;
                return View("Login", model);
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
                    EnderecoId = 1,
                    FirstAccess = true
                };

                var result = await _userManager.CreateAsync(user, model.SenhaCadastro);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "AUTOR");

                    //var phoneNumber = "+55012991206314";
                    //var phoneToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
                    //await _smsService.SendAccountConfirmation(phoneNumber, phoneToken);

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
        public async Task<IActionResult> ConfirmEmail(string user, string code)
        {
            try
            {
                if (user != null && code != null)
                {

                    var findUser = await _userManager.FindByNameAsync(user);
                    var result = await _userManager.ConfirmEmailAsync(findUser, code);


                    if (result.Succeeded)
                    {
                        return View();

                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar() 
        {
            try
            {
                RegistrarViewModel model = new RegistrarViewModel();
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var estados = await _localizacaoServices.GetEstado();
                model.Estados = estados;
                model.EmailPrincipal = user.Email;
                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar(RegistrarViewModel model)
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
