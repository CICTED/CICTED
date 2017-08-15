using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.ViewModels.Administrador;
using System.Collections.Generic;
using System;
using CICTED.Domain.Infrastucture.Services.Interfaces;
using CICTED.Domain.Infrastucture.Helpers;
using CICTED.Domain.Infrastucture.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("administrador")]
    public class AdministradorController : Controller
    {
        private static string urlRoot = "http://localhost:54134";
        private UserManager<ApplicationUser> _userManager;
        private IAccountRepository _accountRepository;
        private ITrabalhoRepository _trabalhoRepository;
        private IEventoRepository _eventoRepository;
        private IAreaRepository _areaRepository;
        private IAutorRepository _autorRepository;
        private IAgenciaRepository _agenciaRepository;
        private ILocalizacaoRepository _localizacaoRepository;
        private IAdministradorRepository _administradorRepository;
        private IEmailServices _emailServices;
        private IAdministradorServices _administradorServices;
        private ILocalizacaoServices _localizacaoServices;


        public AdministradorController(ITrabalhoRepository trabalhoRepository, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository, IEventoRepository eventoRepository, IAreaRepository areaRepository, IAutorRepository autorRepository, IAgenciaRepository agenciaRepository, IAdministradorRepository administradorRepository, ILocalizacaoRepository localizacaoRepository, IEmailServices emailServices, IAdministradorServices administradorServices, ILocalizacaoServices localizacaoServices)
        {
            _trabalhoRepository = trabalhoRepository;
            _userManager = userManager;
            _accountRepository = accountRepository;
            _eventoRepository = eventoRepository;
            _areaRepository = areaRepository;
            _autorRepository = autorRepository;
            _agenciaRepository = agenciaRepository;
            _administradorRepository = administradorRepository;
            _localizacaoRepository = localizacaoRepository;
            _emailServices = emailServices;
            _administradorServices = administradorServices;
            _localizacaoServices = localizacaoServices;
        }

        [HttpGet("gerenciarOrganizador")]
        [Authorize]
        public async Task<IActionResult> GerenciarOrganizador()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var organizadores = await _administradorServices.GetOrganizador();
            List<Gerenciar> model = new List<Gerenciar>();

            foreach (var organizador in organizadores)
            {
                var isAvaliador = await _administradorRepository.IsAvaliador(organizador.Id);

                var organizadorConsulta = new Gerenciar()
                {
                    Id = organizador.Id,
                    Nome = organizador.Nome,
                    Sobrenome = organizador.Sobrenome,
                    PhoneNumber = organizador.PhoneNumber,
                    Celular = organizador.Celular,
                    DataNascimento = organizador.DataNascimento,
                    Email = organizador.Email,
                    Genero = organizador.Genero,
                    CPF = organizador.CPF,
                    Avaliador = isAvaliador,
                    FirstAccess = organizador.FirstAccess
                };
                model.Add(organizadorConsulta);
            }

            return View(model);

        }

        [HttpGet("cadastrarOrganizador")]
        [Authorize]
        public async Task<IActionResult> CadastrarOrganizador()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var areaConhecimento = await _areaRepository.GetAreas();
                var eventos = await _eventoRepository.getEventos();

                Gerenciar model = new Gerenciar()
                {
                    AreaConhecimento = areaConhecimento,
                    Eventos = eventos,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cadastrarOrganizador")]
        [Authorize]
        public async Task<IActionResult> CadastrarOrganizador(Gerenciar model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    NormalizedEmail = model.Email.ToUpper(),
                    UserName = model.Email,
                    NormalizedUserName = model.Email.ToUpper(),
                    DataCadastro = DateTime.Now,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    PhoneNumber = model.PhoneNumber,
                    Celular = model.Celular,
                    CursosId = 1,
                    InstituicaoId = 1,
                    EnderecoId = 1,
                    FirstAccess = true
                };

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "ORGANIZADOR");

                    if (model.Avaliador == true)
                    {
                        await _userManager.AddToRoleAsync(user, "AVALIADOR");
                        var evento = await _administradorRepository.InsertAvaliadorEvento(model.EventoId, user.Id);
                        var subArea = await _administradorRepository.InsertAvaliadorSubArea(model.SubAreaConhecimentoId, user.Id);
                    }

                    //link
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                       "ConfirmEmail", "Account",
                       new { user = user.UserName, code = code });

                    var url = $"{urlRoot}{callbackUrl}";

                    //email
                    var email = await _emailServices.EnviarEmail(user.Email, url);

                    ViewBag.Cadastrado = "cadastrado";
                    return View("GerenciarOrganizador", new List<Gerenciar>());
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                    ViewBag.Errors = result.ConvertToHTML();
                    return View("CadastrarOrganizador", model);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("gerenciarAvaliador")]
        [Authorize]
        public async Task<IActionResult> GerenciarAvaliador()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var avaliadores = await _administradorServices.GetAvaliador();

            List<Gerenciar> model = new List<Gerenciar>();

            foreach (var avaliador in avaliadores)
            {
                var evento = await _administradorServices.GetEvento(avaliador.Id);

                var subAreaConhecimento = await _administradorServices.GetSubAreaConhecimento(avaliador.Id);

                var avaliadorConsulta = new Gerenciar()
                {
                    Id = avaliador.Id,
                    Nome = avaliador.Nome,
                    Sobrenome = avaliador.Sobrenome,
                    PhoneNumber = avaliador.PhoneNumber,
                    Email = avaliador.Email,
                    Evento = evento,
                    SubAreaConhecimento = subAreaConhecimento,
                    FirstAccess = avaliador.FirstAccess
                };
                model.Add(avaliadorConsulta);
            }

            return View(model);
        }

        [HttpGet("cadastrarAvaliador")]
        [Authorize]
        public async Task<IActionResult> CadastrarAvaliador()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var areaConhecimento = await _areaRepository.GetAreas();
                var eventos = await _eventoRepository.getEventos();

                Gerenciar model = new Gerenciar()
                {
                    AreaConhecimento = areaConhecimento,
                    Eventos = eventos,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cadastrarAvaliador")]
        [Authorize]
        public async Task<IActionResult> CadastrarAvaliador(Gerenciar model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (model.Email == model.ConfirmaEmail)
                {
                    var user = new ApplicationUser
                    {
                        Email = model.Email,
                        NormalizedEmail = model.Email.ToUpper(),
                        UserName = model.Email,
                        NormalizedUserName = model.Email.ToUpper(),
                        DataCadastro = DateTime.Now,
                        Nome = model.Nome,
                        Sobrenome = model.Sobrenome,
                        PhoneNumber = model.PhoneNumber,
                        Celular = model.Celular,
                        CursosId = 1,
                        InstituicaoId = 1,
                        EnderecoId = 1,
                        FirstAccess = true
                    };

                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "AVALIADOR");

                        //link
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action(
                           "ConfirmEmail", "Account",
                           new { user = user.UserName, code = code });

                        var url = $"{urlRoot}{callbackUrl}";

                        //email
                        var email = await _emailServices.EnviarEmail(user.Email, url);

                        ViewBag.Cadastrado = "cadastrado";
                        return View("GerenciarAvaliador", new List<Gerenciar>());

                    }
                    else
                    {
                        await _userManager.DeleteAsync(user);
                        ViewBag.Errors = result.ConvertToHTML();
                        return View("CadastrarAvaliador", model);
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

        [HttpGet("gerenciarAutor")]
        [Authorize]
        public async Task<IActionResult> GerenciarAutor()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var autores = await _administradorServices.GetAutor();
            List<Gerenciar> model = new List<Gerenciar>();

            foreach (var autor in autores)
            {

                var autorConsulta = new Gerenciar()
                {
                    Id = autor.Id,
                    Nome = autor.Nome,
                    Sobrenome = autor.Sobrenome,
                    PhoneNumber = autor.PhoneNumber,
                    Email = autor.Email,
                    FirstAccess = autor.FirstAccess
                };
                model.Add(autorConsulta);
            }

            return View(model);
        }


        [HttpGet("informacaoOrganizador")]
        [Authorize]
        public async Task<IActionResult> InformacaoOrganizador(long id)
        {
            Gerenciar organizadores = await _administradorRepository.GetOrganizador(id);

            Gerenciar endereco = await _localizacaoRepository.GetEndereco(organizadores.EnderecoId);
            var cidade = await _localizacaoRepository.GetCidade(endereco.CidadeId);
            var estado = await _localizacaoServices.GetEstado(cidade.Id);
            var isAvaliador = await _administradorRepository.IsAvaliador(organizadores.Id);
            var evento = await _administradorServices.GetEventoAvaliador(organizadores.Id);
            var subarea = await _administradorServices.GetAvaliadorSubArea(organizadores.Id);

            var model = new Gerenciar()
            {
                Nome = organizadores.Nome,
                Sobrenome = organizadores.Sobrenome,
                PhoneNumber = organizadores.PhoneNumber,
                Celular = organizadores.Celular,
                CPF = organizadores.CPF,
                Email = organizadores.Email,
                Nascimento = organizadores.DataNascimento.ToString("dd/MM/yyyy"),
                Genero = organizadores.Genero,
                Avaliador = isAvaliador,
                Logradouro = endereco.Logradouro,
                Bairro = endereco.Bairro,
                CidadeNome = cidade.CidadeNome,
                Sigla = estado.Sigla,
                Numero = endereco.Numero,
                Evento = evento,
                SubAreaConhecimento = subarea
            };

            return Json(model);
        }

        [HttpGet("informacaoAvaliador")]
        [Authorize]
        public async Task<IActionResult> InformacaoAvaliador(long id)
        {
            Gerenciar avaliadores = await _administradorRepository.GetAvaliador(id);
            Gerenciar endereco = await _localizacaoRepository.GetEndereco(avaliadores.EnderecoId);
            var cidade = await _localizacaoRepository.GetCidade(endereco.CidadeId);
            var estado = await _localizacaoServices.GetEstado(cidade.Id);
            var eventos = await _administradorServices.GetEvento(id);
            var subArea = await _administradorServices.GetSubAreaConhecimento(id);

            var model = new Gerenciar()
            {
                Nome = avaliadores.Nome,
                Sobrenome = avaliadores.Sobrenome,
                Email = avaliadores.Email,
                PhoneNumber = avaliadores.PhoneNumber,
                Celular = avaliadores.Celular,
                CPF = avaliadores.CPF,
                Nascimento = avaliadores.DataNascimento.ToString("dd/MM/yyyy"),
                Genero = avaliadores.Genero,
                Logradouro = endereco.Logradouro,
                Bairro = endereco.Bairro,
                CidadeNome = cidade.CidadeNome,
                Sigla = estado.Sigla,
                Numero = endereco.Numero,
                Evento = eventos,
                SubAreaConhecimento = subArea
            };

            return Json(model);
        }

        [HttpGet("informacaoAutor")]
        [Authorize]
        public async Task<IActionResult> InformacaoAutor(long id)
        {
            Gerenciar autores = await _administradorRepository.GetAutor(id);
            Gerenciar endereco = await _localizacaoRepository.GetEndereco(autores.EnderecoId);
            var instituicao = await _administradorRepository.GetInstituicao(autores.InstituicaoId);
            var cidade = await _localizacaoRepository.GetCidade(endereco.CidadeId);
            var estado = await _localizacaoServices.GetEstado(cidade.Id);

            var model = new Gerenciar()
            {
                Nome = autores.Nome,
                Sobrenome = autores.Sobrenome,
                PhoneNumber = autores.PhoneNumber,
                Celular = autores.Celular,
                CPF = autores.CPF,
                Email = autores.Email,
                Nascimento = autores.DataNascimento.ToString("dd/MM/yyyy"),
                Genero = autores.Genero,
                Logradouro = endereco.Logradouro,
                Bairro = endereco.Bairro,
                CidadeNome = cidade.CidadeNome,
                Sigla = estado.Sigla,
                Numero = endereco.Numero,
                InstituicaoNome = instituicao
            };

            return Json(model);
        }

        [HttpGet("list/subarea/{areaId}")]
        public async Task<IActionResult> Subarea(List<int> areaId)
        {
            var subAreas = await _areaRepository.GetSubAreass(areaId);

            if (subAreas == null)
            {
                return BadRequest("There was an error to load the subAreas.");
            }
            return Json(subAreas);
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(string id)
        {
            try
            {
                var deletar = await _administradorRepository.Excluir(id);

                if (deletar == true)
                {
                    return Ok();
                }
                return BadRequest("Não foi possível excluir");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
