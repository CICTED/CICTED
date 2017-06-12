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

                var organizadorConsulta = new GerenciarOrganizador()
                {
                    Id = organizador.Id,
                    Nome = organizador.Nome,
                    Sobrenome = organizador.Sobrenome,
                    Telefone = organizador.Telefone,
                    Nascimento = organizador.Nascimento,
                    Email = organizador.Email,
                    Genero = organizador.Genero,
                    Celular = organizador.Celular,
                    CPF = organizador.CPF, 
                    Avaliador = organizador.Avaliador
                };
                model.Add(organizadorConsulta);
            }

            return View(model);
            
        }
    }

}
