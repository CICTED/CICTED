﻿using System;
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


        [HttpGet("trabalhos")]
        public async Task<IActionResult>Trabalhos(int idEvento)
        {
            DashboardViewModel model = new DashboardViewModel();

            model.TrabalhosBiologicas = await _organizadorServices.GetQuantidadeTrabalhosArea(1);
            model.TrabalhosExatas = await _organizadorServices.GetQuantidadeTrabalhosArea(2);
            model.TrabalhosHumanas = await _organizadorServices.GetQuantidadeTrabalhosArea(3);

            model.AvaliadosBiologicas = await _organizadorServices.GetQuantidadeTrabalhosAvaliadosArea(1);
            model.AvaliadosExatas = await _organizadorServices.GetQuantidadeTrabalhosAvaliadosArea(2);
            model.AvaliadosHumanas = await _organizadorServices.GetQuantidadeTrabalhosAvaliadosArea(3);

            model.Cadastrados = model.TrabalhosBiologicas + model.TrabalhosExatas + model.TrabalhosHumanas;
            model.Avaliados = model.AvaliadosBiologicas + model.AvaliadosExatas + model.AvaliadosHumanas;
            model.Submetidos = await _organizadorRepository.GetQuantidadeTrabalhosSubmetidos();


            model.Aprovados = await _organizadorRepository.GetQuantidadeTrabalhosAprovados();
            model.Reprovados = await _organizadorRepository.GetQuantidadeTrabalhosReprovados();

            return Json(model);
        }

        
    }
}
