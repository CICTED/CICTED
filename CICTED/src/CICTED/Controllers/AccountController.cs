using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.ViewModels.Account;
using CICTED.Domain.Infrastucture.Helpers;
using CICTED.Domain.Infrastucture.Services.Interfaces;
using CICTED.Domain.Entities.Localizacao;
using CICTED.Domain.Infrastucture.Repository.Interfaces;

namespace CICTED.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private IEmailServices _emailServices;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private ILocalizacaoRepository _localizacaoRepository;
        private ISmsService _smsService;
        private IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailServices emailServices, ILocalizacaoRepository localizacaoRepository, ISmsService smsService)
        {
            _localizacaoRepository = localizacaoRepository;
            _emailServices = emailServices;
            _signInManager = signInManager;
            _userManager = userManager;
            _smsService = smsService;
            _accountRepository = accountRepository;
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

                    var result = await _signInManager.PasswordSignInAsync(model.EmailLogin,
                       model.SenhaLogin, model.RememberMe, user.EmailConfirmed);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
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

                            ViewBag.EnviadoEmail = true;
                            return View("Login", new LoginViewModel());
                        }
                        else if (model.ConfirmaEmail == false && user.EmailConfirmed == false)
                        {
                            ViewBag.EmailNaoConfirmado = true;
                            return View("Login", new LoginViewModel());
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
                var estados = await _localizacaoRepository.GetEstado();
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
        public async Task<IActionResult> Registrar(RegistrarViewModel model)
        {
            try
            {
                long cidadeId = 0;
                long enderecoId = 0;
                var endereco = new Endereco
                {
                    Bairro = model.Bairro,
                    CEP = model.CEP,
                    Complemento = model.Complemento,
                    Logradouro = model.Logradouro,
                    Numero = model.Numero,
                };


                if (model.EnderecoExterior == true)
                {
                    var enderecoExterior = new EnderecoExterior()
                    {
                        Cidade = model.CidadeExterior,
                        Estado = model.EstadoExterior,
                        Pais = model.Pais
                    };

                    cidadeId = await _localizacaoRepository.InsertEnderecoExterior(enderecoExterior);
                    enderecoId = await _localizacaoRepository.InsertEndereco(endereco, 0, cidadeId);

                }
                else
                {
                    cidadeId = model.CidadeId;
                    enderecoId = await _localizacaoRepository.InsertEndereco(endereco, cidadeId);
                }

                

                var user = new RegistrarViewModel()
                {
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    CPF = model.CPF,
                    DataNascimento = model.DataNascimento,
                    Documento = model.Documento,
                    Genero = model.Genero,
                    Telefone = model.Telefone,
                    Celular = model.Celular,
                    EmailSecundario = model.EmailSecundario,
                    Instituicao = model.Instituicao,
                    Bolsista = model.Bolsista,
                    Estudante = model.Estudante,
                    
                };


                //var result = await _accountRepository.UpdateDadosUsuario(user);

                //if (result.Succeeded)
                //{
                //    model.ReturnMessage = "Alterações salvas com sucesso";
                //    return View("Login", new LoginViewModel());
                //}

                //else
                //{
                //    ViewBag.Errors = result.ConvertToHTML();
                //    return View("Register", model);
                //}

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
