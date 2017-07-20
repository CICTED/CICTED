using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.ViewModels.Trabalho;
using CICTED.Domain.Infrastucture.Repository.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    public class DashboardController : Controller
    {

       private ITrabalhoRepository _trabalhoRepository;
        private IDashboardRepository _dashboardRepository;

        public DashboardController(ITrabalhoRepository trabalhoRepository, IDashboardRepository dashboardRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _dashboardRepository = dashboardRepository;
        }


        // GET: /<controller>/
        [HttpGet("Dashboard")]
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                DashboardViewModel model = new DashboardViewModel();
                var cadastrados = await _dashboardRepository.GetQuantidadeDatasCadastrados();
                var submetidos = await _dashboardRepository.GetQuantidadeDatasSubmetidos();
                var avaliados = await _dashboardRepository.GetQuantidadeDataAvaliacao();
                return View(model);
            }
        }

        public async Task<IActionResult> GraficoTrabalhos(int idEvento)
        {
            var cadastrados = await _dashboardRepository.GetQuantidadeDatasCadastrados(idEvento);
            var submetidos = await _dashboardRepository.GetQuantidadeDatasSubmetidos(idEvento);
            var avaliados = await _dashboardRepository.GetQuantidadeDataAvaliacao(idEvento);

            return View();
        }

    }
}
