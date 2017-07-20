using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.ViewModels.Trabalho;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.Infrastucture.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("organizador")]
    public class OrganizadorController : Controller
    {

        private ITrabalhoRepository _trabalhoRepository;
        private IOrganizadorRepository _organizadorRepository;
        private IOrganizadorServices _organizadorServices;

        public OrganizadorController(IOrganizadorServices organizadorServices, ITrabalhoRepository trabalhoRepository, IOrganizadorRepository dashboardRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _organizadorRepository = dashboardRepository;
            _organizadorServices = organizadorServices;
        }


        // GET: /<controller>/
        [HttpGet("dashboard")]
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var totalCadastrados = 0;
                var totalAvaliados = 0;
                var totalSubmetidos = 0;

                DashboardViewModel model = new DashboardViewModel();
                var cadastrados = await _organizadorRepository.GetQuantidadeDatasCadastrados();
                var submetidos = await _organizadorRepository.GetQuantidadeDatasSubmetidos();
                var avaliados = await _organizadorServices.GetQuantidadeDataAvaliacao();

                foreach(var conta in cadastrados)
                {
                    totalCadastrados += conta.Quantidade;
                }

                model.Cadastrados = cadastrados.Count();
                model.Avaliados = avaliados.Count();
                model.Submetidos = submetidos.Count();
                return View("Dashboard", model);
            }
        }

        public async Task<IActionResult> GraficoTrabalhos(int idEvento)
        {
            var cadastrados = await _organizadorRepository.GetQuantidadeDatasCadastrados();
            var submetidos = await _organizadorRepository.GetQuantidadeDatasSubmetidos();
            var avaliados = await _organizadorServices.GetQuantidadeDataAvaliacao();

            return View();
        }

    }
}
