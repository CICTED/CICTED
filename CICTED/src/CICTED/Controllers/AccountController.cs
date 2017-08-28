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
using System.Text;
using System.Security.Cryptography;

namespace CICTED.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private static string urlRoot = "http://localhost:54134";
        private IEmailServices _emailServices;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private ILocalizacaoRepository _localizacaoRepository;
        private ISmsService _smsService;
        private IAccountRepository _accountRepository;
        private ITrabalhoRepository _trabalhoRepository;
        private IAutorRepository _autorRepository;
        private ILocalizacaoServices _localizacaoServices;

        public AccountController(ITrabalhoRepository trabalhoRepository, IAccountRepository accountRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailServices emailServices, ILocalizacaoRepository localizacaoRepository, ISmsService smsService, IAutorRepository autorRepository, ILocalizacaoServices localizacaoServices)
        {
            _localizacaoRepository = localizacaoRepository;
            _emailServices = emailServices;
            _signInManager = signInManager;
            _userManager = userManager;
            _smsService = smsService;
            _accountRepository = accountRepository;
            _trabalhoRepository = trabalhoRepository;
            _autorRepository = autorRepository;
            _localizacaoServices = localizacaoServices;
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
                //_userManager.GeneratePasswordResetTokenAsync

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
                        ViewBag.LoginError = "Usuário ou senha incorretas";
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
                    try
                    {
                       var resultRole =  await _userManager.AddToRoleAsync(user, "AUTOR");

                        //link
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action(
                           "ConfirmEmail", "Account",
                           new { user = user.UserName, code = code });

                        var url = $"{urlRoot}{callbackUrl}";

                        //email
                        var email = await _emailServices.EnviarEmail(user.Email, url);

                        ViewBag.Cadastrado = "cadastrado";
                        return View("Login", new LoginViewModel());
                    }
                    catch (Exception ex)
                    {
                        await _userManager.DeleteAsync(user);
                        return BadRequest(ex.Message);
                    }
                }
                else
                {
                    //await _userManager.DeleteAsync(user);
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> DadosUsuario()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var endereco = await _accountRepository.GetEndereco(user.EnderecoId);
                var estados = await _localizacaoRepository.GetEstados();
                var estado = await _localizacaoServices.GetEstado(endereco.CidadeId);
                var cursos = await _accountRepository.GetCursos();
                var cidade = await _localizacaoRepository.GetCidade(endereco.CidadeId);
                var cidades = await _localizacaoRepository.GetCidades(estado.Id);

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
                model.Cidades = cidades;
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

        [HttpPost("meusdados")]
        public async Task<IActionResult> DadosUsuario(DadosUsuárioViewModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var idUsuario = user.Id;

                if (validarCPF(model.CPF) == false)
                {
                    ViewBag.CPF = true;
                    return RedirectToAction("DadosUsuario");
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

                long cidadeId = 0;
                long enderecoId = user.EnderecoId;
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

                    cidadeId = await _localizacaoRepository.UpdateEnderecoExterior(enderecoExterior);
                    enderecoId = await _localizacaoRepository.UpdateEndereco(endereco, 0, cidadeId, enderecoId);

                }
                else
                {
                    cidadeId = model.CidadeId;
                    enderecoId = await _localizacaoRepository.UpdateEndereco(endereco, cidadeId, enderecoId, 0);

                }

                var result = await _accountRepository.UpdateDadosUsuario(usuarioDados, user.EnderecoId, idUsuario);

                if (result == true)
                {
                    model.ReturnMessage = "Alterações salvas com sucesso";
                    return RedirectToAction("DadosUsuario");
                }

                else
                {
                    return View("DadosUsuario", model);
                }

                return Ok();
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



        [HttpGet("verifica/usuario")]
        public async Task<IActionResult> VerificaUsuario(string email)
        {
            try
            {
                var usuario = await _accountRepository.BuscaUsuario(email);
                if (usuario != null)
                {
                    var status = await _autorRepository.GetStatusAutor(usuario.Id);
                    var autor = new AutorViewModel()
                    {
                        Email = usuario.Email,
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Sobrenome = usuario.Sobrenome,
                        StatusId = status,
                    };
                    return Json(autor);
                }
                else
                {
                    return Ok();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("alterarSenha")]
        [Authorize]
        public async Task<IActionResult> AlterarSenha()
        {
            return View();
        }

        [HttpPost("alterarsenha")]
        public async Task<IActionResult> AlterarSenha(AlterarSenhaViewModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                
                var senhaAtual = user.PasswordHash;

                var result = await _signInManager.PasswordSignInAsync(user.Email,
                     model.SenhaAtual, user.EmailConfirmed, user.EmailConfirmed);
                var novaSenha = model.NovaSenha;

                if (result.Succeeded)
                {
                    if (novaSenha == model.ConfirmarSenha)
                    {
                        
                        var bite = Encoding.UTF8.GetBytes(novaSenha);
                        using(var hash = SHA256.Create())
                        {
                            var hashedInputBytes = hash.ComputeHash(bite);
                            var hashedInputStringBuilder = new StringBuilder(128);
                            string hexNumber = "";
                            foreach(var b in hashedInputBytes)
                            {
                                hexNumber += String.Format("{0:X2}", b);
                            }
                            string senhaa = hexNumber;
                            var senha = await _accountRepository.UpdateSenha(senhaa, user.Id);
                        }
                        ViewData["Messagen"] = "A senha foi alterada.";
                        return View("alterarsenha", model);
                    }
                    else
                    {
                        ViewData["Messager"] = "A senha digitada é diferente da senha confirmada.";
                        return View("alterarsenha", model);
                    }

                }
                else
                {
                    ViewData["Message"] = "A senha é diferente da senha atual.";
                    return View("alterarsenha", model);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("recuperar")]
        public async Task<IActionResult> RecuperarSenha()
        {
            return View();
        }

        [HttpPost("recuperar")]
        public async Task<IActionResult> RecuperarSenha(RecuperarSenhaViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ViewBag.Message = "Usuário não encontrado para o email fornecido.";
                    return View("RecuperarSenha", model);
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ForgotPasswordConfirmation", "Account", new { email = user.Email, code = code }, protocol: HttpContext.Request.Scheme);
                await _emailServices.EnviarEmail(model.Email, callbackUrl, "Recuperar conta - CICTED");

                ViewBag.Success = $"Um email foi enviado para que você possa realizar a recuperação de conta. Acesse seu endereço de email {model.Email}.";
                return View("RecuperarSenha");
            }
            catch(Exception ex)
            {
                ViewBag.Message = "Usuário não encontrado para o email fornecido.";
                return View("RecuperarSenha", model);
            }
        }

        [HttpGet("ForgotPasswordConfirmation")]
        public async Task<IActionResult> ForgotPasswordConfirmation(string email, string code)
        {
            return View(new ForgotPasswordConfirmationViewModel()
            {
                Email = email,
                Token = code
            });
        }

        [HttpPost("ForgotPasswordConfirmation")]
        public async Task<IActionResult> ForgotPasswordConfirmation(ForgotPasswordConfirmationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("ForgotPasswordConfirmation", model);
                }

                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    ModelState.AddModelError("password", "A senha e a confirmação de senha não batem.");
                    return View("ForgotPasswordConfirmation", model);
                }

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("email", "Não foi encontrado nenhum usuário com o email fornecido.");
                    return View("ForgotPasswordConfirmation", model);
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

                if(!result.Succeeded)
                {
                    model.Succeeded = false;
                    model.Message = result.Errors.ToString();
                }
                else
                {
                    model.Succeeded = true;
                    model.Message = "Senha alterada com sucesso!";
                }

                return View("ForgotPasswordConfirmation", model);
            }
            catch(Exception ex)
            {
                model.Succeeded = false;
                model.Message = "Houve um erro interno e não foi possível alterar a senha. Tente novamente.";
                return View("ForgotPasswordConfirmation", model);
            }
        }
    }
}

