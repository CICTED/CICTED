using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using CICTED.Domain.Infrastucture.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.ViewModels.Trabalho;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("avaliador")]
    public class AvaliadorController : Controller
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
        private ITrabalhoServices _trabalhoServices;


        public AvaliadorController(ITrabalhoRepository trabalhoRepository, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository, IEventoRepository eventoRepository, IAreaRepository areaRepository, IAutorRepository autorRepository, IAgenciaRepository agenciaRepository, IAdministradorRepository administradorRepository, ILocalizacaoRepository localizacaoRepository, IEmailServices emailServices, IAdministradorServices administradorServices, ILocalizacaoServices localizacaoServices, ITrabalhoServices trabalhoServices)
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
            _trabalhoServices = trabalhoServices;
        }

        [HttpGet("home")]
        [Authorize]
        public async Task<IActionResult> Home()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return View();
        }

        [HttpGet("trabalhosPendentes")]
        [Authorize]
        public async Task<IActionResult> TrabalhosPendentes()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var trabalhos = await _trabalhoServices.TrabalhosPendentes(user.Id);

            List<AvaliacaoTrabalhoViewModel> model = new List<AvaliacaoTrabalhoViewModel>();

            foreach (var trabalho in trabalhos)
            {
                var evento = await _eventoRepository.GetEvento(trabalho.EventoId);
                var trabalhoConsulta = new AvaliacaoTrabalhoViewModel()
                {
                    IdentificadorTrabalho = trabalho.IdentificadorTrabalho,
                    TituloTrabalho = trabalho.TituloTrabalho,
                    EventoNome = evento.Sigla
                };
                model.Add(trabalhoConsulta);
            }

            return View();
        }

        [HttpGet("avaliarTrabalho")]
        [Authorize]
        public async Task<IActionResult> AvaliarTrabalho(string identificacao)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            AvaliacaoTrabalhoViewModel model = new AvaliacaoTrabalhoViewModel();

            var identificacaoTrabalho = new AvaliacaoTrabalhoViewModel()
            {
                IdentificadorTrabalho = identificacao
            };

            return View();

        }

    }
}
