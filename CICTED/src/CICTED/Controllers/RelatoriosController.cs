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


        [HttpGet("trabalhos")]
        public async Task<IActionResult>Trabalhos(int idEvento)
        {
            DashboardViewModel model = new DashboardViewModel();

            var submetidos = await _organizadorRepository.GetQuantidadeDatasSubmetidos(idEvento);          

            model.TrabalhosBiologicas = await _organizadorServices.GetQuantidadeTrabalhos(1,idEvento);
            model.TrabalhosExatas = await _organizadorServices.GetQuantidadeTrabalhos(2,idEvento);
            model.TrabalhosHumanas = await _organizadorServices.GetQuantidadeTrabalhos(3, idEvento);
            model.Cadastrados = model.TrabalhosBiologicas + model.TrabalhosExatas + model.TrabalhosHumanas;

            var totalSubmetidos = 0;          

            foreach (var trabalho in submetidos)
            {
                totalSubmetidos += trabalho.Quantidade;
            }
          
                      
            model.Submetidos = totalSubmetidos;

            return Json(model);
        }

        
    }
}
