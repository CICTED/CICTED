using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.Infrastucture.Services.Interfaces;
using CICTED.Domain.ViewModels.Organizador;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("relatorios")]
    public class RelatoriosController : Controller
    {
        private ITrabalhoRepository _trabalhoRepository;
        private IOrganizadorRepository _organizadorRepository;
        private IOrganizadorServices _organizadorServices;

        public RelatoriosController(IOrganizadorServices organizadorServices, ITrabalhoRepository trabalhoRepository, IOrganizadorRepository dashboardRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _organizadorRepository = dashboardRepository;
            _organizadorServices = organizadorServices;
        }
        [HttpGet("trabalhos/cadastrados")]
        public async Task<IActionResult> TrabalhosCadastrados(int idEvento)
        {
            var cadastrados = await _organizadorRepository.GetQuantidadeDatasCadastrados(idEvento);
                       
            return Json(cadastrados);
        }
        [HttpGet("trabalhos/submetidos")]
        public async Task<IActionResult> TrabalhosSubmetidos(int idEvento)
        {
            var submetidos = await _organizadorRepository.GetQuantidadeDatasSubmetidos(idEvento);

            return Json(submetidos);
        }
        [HttpGet("trabalhos/avaliados")]
        public async Task<IActionResult> TrabalhosAvaliados(int idEvento)
        {
            var avaliados = await _organizadorServices.GetQuantidadeDataAvaliacao(idEvento);

            return Json(avaliados);
        }

        [HttpGet("trabalhos")]
        public async Task<IActionResult>Trabalhos(int idEvento, DashboardViewModel model)
        {
            var cadastrados = await _organizadorRepository.GetQuantidadeDatasCadastrados(idEvento);
            var submetidos = await _organizadorRepository.GetQuantidadeDatasSubmetidos(idEvento);
            var avaliados = await _organizadorServices.GetQuantidadeDataAvaliacao(idEvento);

            model.TrabalhosBiologicas = await _organizadorRepository.GetQuantidadeTrabalhos(1);
            model.TrabalhosExatas = await _organizadorRepository.GetQuantidadeTrabalhos(2);
            model.TrabalhosHumanas = await _organizadorRepository.GetQuantidadeTrabalhos(3);

            model.Cadastrados = cadastrados.Count();
            model.Submetidos = submetidos.Count();
            model.Avaliados = avaliados.Count();

            return Json(model);
        }

        
    }
}
