using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.ViewModels.Trabalho;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.Infrastucture.Services.Interfaces;
using CICTED.Domain.ViewModels.Organizador;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using CICTED.Domain.ViewModels.Administrador;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("organizador")]
    public class OrganizadorController : Controller
    {

        private ITrabalhoRepository _trabalhoRepository;
        private IOrganizadorRepository _organizadorRepository;
        private IOrganizadorServices _organizadorServices;
        private UserManager<ApplicationUser> _userManager;
        private IAdministradorRepository _administradorRepository;
        private ILocalizacaoRepository _localizacaoRepository;
        private ILocalizacaoServices _localizacaoServices;

        public OrganizadorController(IOrganizadorServices organizadorServices, ITrabalhoRepository trabalhoRepository, IOrganizadorRepository dashboardRepository, UserManager<ApplicationUser> userManager, IAdministradorRepository administradorRepository, ILocalizacaoRepository localizacaoRepository, ILocalizacaoServices localizacaoServices)
        {
            _trabalhoRepository = trabalhoRepository;
            _organizadorRepository = dashboardRepository;
            _organizadorServices = organizadorServices;
            _userManager = userManager;
            _administradorRepository = administradorRepository;
            _localizacaoRepository = localizacaoRepository;
            _localizacaoServices = localizacaoServices;
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
                DashboardViewModel model = new DashboardViewModel();
                                
                var submetidos = await _organizadorRepository.GetQuantidadeDatasSubmetidos();                

                var totalSubmetidos = 0;                


                foreach (var trabalho in submetidos)
                {
                    totalSubmetidos += trabalho.Quantidade;
                }

                model.TrabalhosBiologicas = await _organizadorServices.GetQuantidadeTrabalhos(1);
                model.TrabalhosExatas = await _organizadorServices.GetQuantidadeTrabalhos(2);
                model.TrabalhosHumanas = await _organizadorServices.GetQuantidadeTrabalhos(3);
                                                

                model.Cadastrados = model.TrabalhosBiologicas + model.TrabalhosExatas + model.TrabalhosHumanas;
                
                model.Submetidos = totalSubmetidos;

                return View("Dashboard", model);
            }
        }


        [HttpGet("consultar/usuario")]
        [Authorize]
        public async Task<IActionResult> ConsultarUsuario()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var usuarios = await _organizadorRepository.GetUsuarios();
            List<Gerenciar> model = new List<Gerenciar>();

            foreach (var usuario in usuarios)
            {
                var isAvaliador = await _administradorRepository.IsAvaliador(usuario.Id);

                var usuarioConsulta = new Gerenciar()
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Sobrenome = usuario.Sobrenome,
                    PhoneNumber = usuario.PhoneNumber,
                    Celular = usuario.Celular,
                    DataNascimento = usuario.DataNascimento,
                    Email = usuario.Email,
                    Genero = usuario.Genero,
                    CPF = usuario.CPF,
                    Avaliador = isAvaliador,
                    FirstAccess = usuario.FirstAccess
                };
                model.Add(usuarioConsulta);
            }

            return View(model);
        }


        [HttpGet("informacaoUsuario")]
        [Authorize]
        public async Task<IActionResult> InformacaoUsuario(long id)
        {
            Gerenciar usuarios = await _organizadorRepository.GetUsuarios(id);

            Gerenciar endereco = await _localizacaoRepository.GetEndereco(usuarios.EnderecoId);
            var cidade = await _localizacaoRepository.GetCidade(endereco.CidadeId);
            var estado = await _localizacaoServices.GetEstado(cidade.Id);

            var isAvaliador = await _administradorRepository.IsAvaliador(usuarios.Id);
            var isAdministrador = await _administradorRepository.IsAdministrador(usuarios.Id);
            var isAutor = await _administradorRepository.IsAutor(usuarios.Id);
            var isOrganizador = await _administradorRepository.IsOrganizador(usuarios.Id);

            var model = new Gerenciar()
            {
                Nome = usuarios.Nome,
                Sobrenome = usuarios.Sobrenome,
                PhoneNumber = usuarios.PhoneNumber,
                Celular = usuarios.Celular,
                CPF = usuarios.CPF,
                Email = usuarios.Email,
                Nascimento = usuarios.DataNascimento.ToString("dd/MM/yyyy"),
                Genero = usuarios.Genero,
                Avaliador = isAvaliador,
                Autor = isAutor,
                Administrador = isAdministrador,
                Organizador = isOrganizador,
                Logradouro = endereco.Logradouro,
                Bairro = endereco.Bairro,
                CidadeNome = cidade.CidadeNome,
                Sigla = estado.Sigla,
                Numero = endereco.Numero,
            };

            return Json(model);
        }



    }
}
