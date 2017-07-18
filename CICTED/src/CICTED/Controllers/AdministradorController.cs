using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.ViewModels.Administrador;
using System.Collections.Generic;
using System;

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
        private ILocalizacaoRepository _localizacaoRepository;
        private IAdministradorRepository _administradorRepository;


        public AdministradorController(ITrabalhoRepository trabalhoRepository, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository, IEventoRepository eventoRepository, IAreaRepository areaRepository, IAutorRepository autorRepository, IAgenciaRepository agenciaRepository, IAdministradorRepository administradorRepository, ILocalizacaoRepository localizacaoRepository)
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
        }

        [HttpGet("gerenciarOrganizador")]
        [Authorize]
        public async Task<IActionResult> GerenciarOrganizador()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var organizadores = await _administradorRepository.GetOrganizador();
            List<Gerenciar> model = new List<Gerenciar>();

            foreach (var organizador in organizadores)
            {
                var isAvaliador = await _administradorRepository.IsAvaliador(organizador.Id);

                var organizadorConsulta = new Gerenciar()
                {
                    Id = organizador.Id,
                    Nome = organizador.Nome,
                    Sobrenome = organizador.Sobrenome,
                    PhoneNumber = organizador.PhoneNumber,
                    Celular = organizador.Celular,
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

        [HttpGet("cadastrarOrganizador")]
        [Authorize]
        public async Task<IActionResult> CadastrarOrganizador()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("gerenciarAvaliador")]
        [Authorize]
        public async Task<IActionResult> GerenciarAvaliador()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var avaliadores = await _administradorRepository.GetAvaliador();

            List<Gerenciar> model = new List<Gerenciar>();

            foreach (var avaliador in avaliadores)
            {
                var evento = await _administradorRepository.GetEvento(avaliador.Id);

                var subAreaConhecimento = await _administradorRepository.GetSubAreaConhecimento(avaliador.Id);

                var avaliadorConsulta = new Gerenciar()
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

        [HttpGet("cadastrarAvaliador")]
        [Authorize]
        public async Task<IActionResult> CadastrarAvaliador()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("gerenciarAutor")]
        [Authorize]
        public async Task<IActionResult> GerenciarAutor()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var autores = await _administradorRepository.GetAutor();
            List<Gerenciar> model = new List<Gerenciar>();

            foreach (var autor in autores)
            {

                var autorConsulta = new Gerenciar()
                {
                    Id = autor.Id,
                    Nome = autor.Nome,
                    Sobrenome = autor.Sobrenome,
                    PhoneNumber = autor.PhoneNumber,
                    Email = autor.Email,
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
            Gerenciar organizadores = await _administradorRepository.GetOrganizador(id);

            Gerenciar endereco = await _localizacaoRepository.GetEndereco(organizadores.EnderecoId);
            var cidade = await _localizacaoRepository.GetCidade(endereco.CidadeId);
            var estado = await _localizacaoRepository.GetEstado(cidade.Id);


            var model = new Gerenciar()
            {
                Nome = organizadores.Nome,
                Sobrenome = organizadores.Sobrenome,
                PhoneNumber = organizadores.PhoneNumber,
                Celular = organizadores.Celular,
                CPF = organizadores.CPF,
                Email = organizadores.Email,
                Nascimento = organizadores.DataNascimento.ToString("dd/MM/yyyy"),
                Genero = organizadores.Genero,
                Avaliador = organizadores.Avaliador,
                Logradouro = endereco.Logradouro,
                Bairro = endereco.Bairro,
                CidadeNome = cidade.CidadeNome,
                Sigla = estado.Sigla,
                Numero = endereco.Numero,
            };

            return Json(model);
        }

        [HttpGet("informacaoAvaliador")]
        [Authorize]
        public async Task<IActionResult> InformacaoAvaliador(long id)
        {
            Gerenciar avaliadores = await _administradorRepository.GetAvaliador(id);
            Gerenciar endereco = await _localizacaoRepository.GetEndereco(avaliadores.EnderecoId);
            var cidade = await _localizacaoRepository.GetCidade(endereco.CidadeId);
            var estado = await _localizacaoRepository.GetEstado(cidade.Id);
            var eventos = await _administradorRepository.GetEvento(id);
            var subArea = await _administradorRepository.GetSubAreaConhecimento(id);

            var model = new Gerenciar()
            {
                Nome = avaliadores.Nome,
                Sobrenome = avaliadores.Sobrenome,
                Email = avaliadores.Email,
                PhoneNumber = avaliadores.PhoneNumber,
                Celular = avaliadores.Celular,
                CPF = avaliadores.CPF,
                Nascimento = avaliadores.DataNascimento.ToString("dd/MM/yyyy"),
                Genero = avaliadores.Genero,
                Logradouro = endereco.Logradouro,
                Bairro = endereco.Bairro,
                CidadeNome = cidade.CidadeNome,
                Sigla = estado.Sigla,
                Numero = endereco.Numero,
                Evento = eventos,
                SubAreaConhecimento = subArea
            };

            return Json(model);
        }

        [HttpGet("informacaoAutor")]
        [Authorize]
        public async Task<IActionResult> InformacaoAutor(long id)
        {
            Gerenciar autores = await _administradorRepository.GetAutor(id);
            Gerenciar endereco = await _localizacaoRepository.GetEndereco(autores.EnderecoId);
            var instituicao = await _administradorRepository.GetInstituicao(autores.InstituicaoId);
            var cidade = await _localizacaoRepository.GetCidade(endereco.CidadeId);
            var estado = await _localizacaoRepository.GetEstado(cidade.Id);

            var model = new Gerenciar()
            {
                Nome = autores.Nome,
                Sobrenome = autores.Sobrenome,
                PhoneNumber = autores.PhoneNumber,
                Celular = autores.Celular,
                CPF = autores.CPF,
                Email = autores.Email,
                Nascimento = autores.DataNascimento.ToString("dd/MM/yyyy"),
                Genero = autores.Genero,
                Logradouro = endereco.Logradouro,
                Bairro = endereco.Bairro,
                CidadeNome = cidade.CidadeNome,
                Sigla = estado.Sigla,
                Numero = endereco.Numero,
                InstituicaoNome = instituicao
            };

            return Json(model);
        }

    }
}
