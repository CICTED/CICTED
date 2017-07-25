using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.Infrastucture.Services.Interfaces;

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
            var cadastrados = await _organizadorRepository.GetQuantidadeDatasCadastrados();
                       
            return Json(cadastrados);
        }
        [HttpGet("trabalhos/submetidos")]
        public async Task<IActionResult> TrabalhosSubmetidos(int IdEvento)
        {
            var submetidos = await _organizadorRepository.GetQuantidadeDatasSubmetidos();

            return Json(submetidos);
        }
        [HttpGet("trabalhos/avaliados")]
        public async Task<IActionResult> TrabalhosAvaliados(int idEvento)
        {
            var avaliados = await _organizadorServices.GetQuantidadeDataAvaliacao();

            return Json(avaliados);
        }
    }
}
