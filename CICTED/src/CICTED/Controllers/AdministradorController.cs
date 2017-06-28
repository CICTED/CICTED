using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.ViewModels.Administrador;
using System.Collections.Generic;

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
        private IAdministradorRepository _administradorRepository;


        public AdministradorController(ITrabalhoRepository trabalhoRepository, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository, IEventoRepository eventoRepository, IAreaRepository areaRepository, IAutorRepository autorRepository, IAgenciaRepository agenciaRepository, IAdministradorRepository administradorRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _userManager = userManager;
            _accountRepository = accountRepository;
            _eventoRepository = eventoRepository;
            _areaRepository = areaRepository;
            _autorRepository = autorRepository;
            _agenciaRepository = agenciaRepository;
            _administradorRepository = administradorRepository;
        }

        [HttpGet("gerenciarOrganizador")]
        [Authorize]
        public async Task<IActionResult> GerenciarOrganizador()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var organizadores = await _administradorRepository.GetOrganizador();
            List<GerenciarOrganizador> model = new List<GerenciarOrganizador>();

            foreach (var organizador in organizadores)
            {
                var isAvaliador = await _administradorRepository.IsAvaliador(organizador.Id);

                var organizadorConsulta = new GerenciarOrganizador()
                {
                    Id = organizador.Id,
                    Nome = organizador.Nome,
                    Sobrenome = organizador.Sobrenome,
                    PhoneNumber = organizador.PhoneNumber,
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

        [HttpGet("gerenciarAvaliador")]
        [Authorize]
        public async Task<IActionResult> GerenciarAvaliador()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var avaliadores = await _administradorRepository.GetAvaliador();

            List<GerenciarAvaliador> model = new List<GerenciarAvaliador>();

            foreach (var avaliador in avaliadores)
            {
                var evento = await _administradorRepository.GetEvento(avaliador.Id);

                var subAreaConhecimento = await _administradorRepository.GetSubAreaConhecimento(avaliador.Id);

                var avaliadorConsulta = new GerenciarAvaliador()
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

        [HttpGet("gerenciarAutor")]
        [Authorize]
        public async Task<IActionResult> GerenciarAutor()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var autores = await _administradorRepository.GetAutor();
            List<GerenciarAutor> model = new List<GerenciarAutor>();

            foreach (var autor in autores)
            {
                var identificacao = await _administradorRepository.GetIdentificacaoTrabalho(autor.Id);

                var autorConsulta = new GerenciarAutor()
                {
                    Id = autor.Id,
                    Nome = autor.Nome,
                    Sobrenome = autor.Sobrenome,
                    PhoneNumber = autor.PhoneNumber,
                    Email = autor.Email,
                    Identificacao = identificacao,
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
            GerenciarOrganizador organizadores = await _administradorRepository.GetOrganizador(id);

            var model = new GerenciarOrganizador()
            {
                Nome = organizadores.Nome,
                Sobrenome = organizadores.Sobrenome,
                PhoneNumber = organizadores.PhoneNumber,
                CPF = organizadores.CPF,
                Email = organizadores.Email,
                DataNascimento = organizadores.DataNascimento,
                Genero = organizadores.Genero,
                Avaliador = organizadores.Avaliador
            };

            return Json(model);
        }

        [HttpGet("informacaoAvaliador")]
        [Authorize]
        public async Task<IActionResult> InformacaoAvaliador(long id)
        {
            GerenciarAvaliador avaliadores = await _administradorRepository.GetAvaliador(id);
            var eventos = await _administradorRepository.GetEvento(id);
            var subArea = await _administradorRepository.GetSubAreaConhecimento(id);

            var model = new GerenciarAvaliador()
            {
                Nome = avaliadores.Nome,
                Sobrenome = avaliadores.Sobrenome,
                Email = avaliadores.Email,
                PhoneNumber = avaliadores.PhoneNumber,
                Evento = eventos,
                SubAreaConhecimento = subArea
            };

            return Json(model);
        }

        [HttpGet("informacaoAutor")]
        [Authorize]
        public async Task<IActionResult> InformacaoAutor(long id)
        {
            GerenciarAutor autores = await _administradorRepository.GetAutor(id);
            var identificacao = await _administradorRepository.GetIdentificacaoTrabalho(id);

            var model = new GerenciarAutor()
            {
                Nome = autores.Nome,
                Sobrenome = autores.Sobrenome,
                PhoneNumber = autores.PhoneNumber,
                Email = autores.Email,
                Identificacao = identificacao
            };

            return Json(model);
        }

    }
}
