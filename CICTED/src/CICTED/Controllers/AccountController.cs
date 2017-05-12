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
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();

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
                            return RedirectToAction("Home");
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
        [Authorize]
        public async Task<IActionResult> Registrar()
        {
            try
            {
                DadosUsuárioViewModel model = new DadosUsuárioViewModel();
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var estados = await _localizacaoRepository.GetEstados();
                var cursos = await _accountRepository.GetCursos();
                model.Instituicoes = await _accountRepository.GetInstituicao();
                model.Estados = estados;
                model.Email = user.Email;
                model.Cursos = cursos;
                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(DadosUsuárioViewModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var idUsuario = user.Id;

                if (validarCPF(model.CPF) == false)
                {
                    ViewBag.CPF = true;
                    return RedirectToAction("Registrar");
                }

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




                var usuarioDados = new DadosUsuárioViewModel()
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
                    InstituicaoId = model.InstituicaoId,
                    Bolsista = model.Bolsista,
                    Estudante = model.Estudante,
                    Email = user.Email,
                    CursoId = (model.CursoId > 0) ? model.CursoId : 1,
                    FirstAccess = false
                };


                var result = await _accountRepository.UpdateDadosUsuario(usuarioDados, enderecoId, idUsuario);

                if (result == true)
                {
                    model.ReturnMessage = "Alterações salvas com sucesso";
                    return RedirectToAction("Home");
                }

                else
                {
                    return View("Registrar", model);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("home")]
        public async Task<IActionResult> Home()
        {
            return View();
        }

        [HttpPost("logoff")]
        public async Task<IActionResult> Logoff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        #region validaCPF
        public static bool validarCPF(string CPF)
        {
            int[] mt1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mt2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string TempCPF;
            string Digito;
            int soma;
            int resto;

            CPF = CPF.Trim();
            CPF = CPF.Replace(".", "").Replace("-", "");

            if (CPF.Length != 11)
                return false;

            TempCPF = CPF.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = resto.ToString();
            TempCPF = TempCPF + Digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = Digito + resto.ToString();

            return CPF.EndsWith(Digito);
        }
        #endregion


        [HttpGet("meusdados")]
        public async Task<IActionResult> DadosUsuario()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var endereco = await _accountRepository.GetEndereco(user.EnderecoId);
                var estados = await _localizacaoRepository.GetEstados();
                var estado = await _localizacaoRepository.GetEstado(endereco.CidadeId);
                var cursos = await _accountRepository.GetCursos();
                var cidade = await _localizacaoRepository.GetCidade(endereco.CidadeId);
                
                DadosUsuárioViewModel model = new DadosUsuárioViewModel();
                model.Nome = user.Nome;
                model.Sobrenome = user.Sobrenome;
                model.CPF = user.CPF;
                model.Documento = user.Documento;
                model.DataNascimento = user.DataNascimento;
                model.Genero = user.Genero;
                model.Telefone = user.PhoneNumber;
                model.Celular = user.Celular;
                model.Email = user.Email;
                model.EmailSecundario = user.EmailSecundario;
                model.Logradouro = endereco.Logradouro;
                model.CidadeId = endereco.CidadeId;
                model.CidadeNome = cidade.CidadeNome;          
                model.Numero = endereco.Numero;
                model.Bairro = endereco.Bairro;
                model.CEP = endereco.CEP;
                model.EstadoID = estado.Id;
                model.EstadoNome = estado.Sigla;
                model.Estados = estados;
                model.Instituicoes = await _accountRepository.GetInstituicao();
                model.Cursos = cursos;
                model.Estudante = user.Estudante;
                model.CursoId = user.CursosId;
                model.Bolsista = user.Bolsista;
                model.InstituicaoId = user.InstituicaoId;

                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

