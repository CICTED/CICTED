using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CICTED.Domain.ViewModels.Autor;
using Microsoft.AspNetCore.Identity;
using CICTED.Domain.Entities.Account;
using CICTED.Domain.Entities.Trabalho;
using CICTED.Domain.ViewModels.Trabalho;
using CICTED.Domain.ViewModels.Account;
using NuGet.Packaging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("trabalho")]
    public class TrabalhoController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IAccountRepository _accountRepository;
        private ITrabalhoRepository _trabalhoRepository;
        private IEventoRepository _eventoRepository;
        private IAreaRepository _areaRepository;

        public TrabalhoController(ITrabalhoRepository trabalhoRepository, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository, IEventoRepository eventoRepository, IAreaRepository areaRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _userManager = userManager;
            _accountRepository = accountRepository;
            _eventoRepository = eventoRepository;
            _areaRepository = areaRepository;
        }

        [HttpGet("cadastro/{IdEvento}")]
        [Authorize]
        public async Task<IActionResult> CadastroTrabalho(int IdEvento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var evento = await _eventoRepository.GetEvento(IdEvento);
            var areas = await _areaRepository.GetAreas();
            var periodos = await _trabalhoRepository.GetPeriodos();
            var agencias = await _trabalhoRepository.GetAgencias();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            AutorViewModel autorPrincipal = new AutorViewModel()
            {
                Email = user.Email,
                Nome = user.Nome.ToUpper(),
                Sobrenome = user.Sobrenome.ToUpper(),
            };

            CadastroTrabalhoViewModel model = new CadastroTrabalhoViewModel()
            {
                Evento = evento,
                AreasConhecimento = areas,
                Periodos = periodos,
                Agencias = agencias,
                AutorPrincipal = autorPrincipal,
            };


            var roles = await _accountRepository.GetRoles(user.Id);
            model.Roles = roles;
            ViewBag.AutorNome = user.Nome;


            return View(model);
        }

        [HttpPost("cadastro/{IdEvento}")]
        public async Task<IActionResult> CadastroTrabalho(CadastroTrabalhoViewModel model, int IdEvento)
        {
            model.Evento = await _eventoRepository.GetEvento(IdEvento);


            string identificacao = await geraIdentificacao(model.Evento);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            model.DataCadastro = DateTime.Now;


            if (await _trabalhoRepository.InsertTrabalho(model.Titulo, model.Introducao, model.Metodologia, model.Resultados, model.Resumo, model.Conclusao, model.Referencias, model.NomeEscola, model.TelefoneEscola, model.CidadeEscola, identificacao, model.DataCadastro, model.TextoCitacao, model.CodigoCEP, model.AgenciaId, model.Evento.Id, model.ArtigoId, model.SubAreaId, model.PeriodoApresentacao))
            {
                model.ReturnMenssagem = "Alterações salvas";
                return View("Account", "Home");
            }
            return View();
        }

        [HttpGet("consulta")]
        [Authorize]
        public async Task<IActionResult> ConsultaTrabalho()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<long> trabalhosId = await _trabalhoRepository.GetTrabalhosId(user.Id);
            List<ConsultaTrabalho> model = new List<ConsultaTrabalho>();

            foreach (var trabalho in trabalhosId)
            {
                model.Add(await _trabalhoRepository.ConsultaTrabalho(trabalho));
            }


            return View(model);
        }

        [HttpGet("consultar")]
        [Authorize]
        public async Task<IActionResult> ConsultaTrabalhoAdm()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var trabalhos = await _trabalhoRepository.GetTrabalho();
            List<ConsultaTrabalho> model = new List<ConsultaTrabalho>();
            foreach (var trabalho in trabalhos)
            {
                var evento = await _eventoRepository.GetEvento(trabalho.EventoId);
                var areaConhecimento = await _areaRepository.GetArea(trabalho.SubAreaConhecimentoId);
                var subAreaConhecimento = await _areaRepository.GetSubArea(trabalho.SubAreaConhecimentoId);

                var trabalhoConsulta = new ConsultaTrabalho()
                {
                    Id = trabalho.Id,
                    EventoNome = evento.Sigla,
                    StatusTrabalhoId = trabalho.StatusTrabalhoId,
                    Identificacao = trabalho.Identificacao,
                    SubAreaConhecimentoNome = subAreaConhecimento,
                    AreaConhecimentoNome = areaConhecimento
                };
                model.Add(trabalhoConsulta);
            }

            return View(model);
        }

        [HttpGet("informacao")]
        [Authorize]
        public async Task<IActionResult> Informacao(long id)
        {
            var trabalho = await _trabalhoRepository.GetInformacaoTrabalho(id);
            var evento = await _eventoRepository.GetEvento(trabalho.EventoId);
            var palavrasChave = await _trabalhoRepository.GetPalavrasChave(id);
            var area = await _areaRepository.GetArea(trabalho.SubAreaConhecimentoId);
            var subArea = await _areaRepository.GetSubArea(trabalho.SubAreaConhecimentoId);
            var status = await _trabalhoRepository.GetStatusTrabalho(trabalho.StatusTrabalhoId);

            List<AutorTrabalho> autoresId = await _trabalhoRepository.GetAutoresId(id);
            List<AutorViewModel> autores = new List<AutorViewModel>() { };

            foreach (var autor in autoresId)
            {
                var info = await _trabalhoRepository.GetAutor(autor.UsuarioId);
                var autorInfo = new AutorViewModel()
                {
                    Email = info.Email,
                    Id = autor.UsuarioId,
                    Nome = info.Nome,
                    Orientador = autor.Orientador,
                    Sobrenome = info.Sobrenome,
                    StatusId = autor.StatusUsuarioId
                };
                autores.Add(autorInfo);
            }

            var model = new InformacoesTrabalhoViewModel()
            {
                Titulo = trabalho.Titulo,
                Identificacao = trabalho.Identificacao,
                Conclusao = trabalho.Conclusao,
                Metodologia = trabalho.Metodologia,
                CidadeEscola = trabalho.CidadeEscola,
                CodigoCEP = trabalho.CodigoCEP,
                DataCadastro = trabalho.DataCadastro,
                Introducao = trabalho.Introducao,
                NomeEscola = trabalho.NomeEscola,
                DataSubmissao = trabalho.DataSubmissao,
                Referencia = trabalho.Referencia,
                Resultado = trabalho.Resultado,
                TelefoneEscola = trabalho.TelefoneEscola,
                Resumo = trabalho.Resumo,
                TextoFinanciadora = trabalho.TextoFinanciadora,
                EventoNome = evento.EventoNome,
                palavrasChave = palavrasChave,
                autores = autores,
                AreaConhecimento = area,
                SubArea = subArea,
                Status = status,
                Id = trabalho.Id,
            };


            return Json(model);
        }


        [HttpGet("list/subarea/{areaId}")]
        public async Task<IActionResult> Subarea(int areaId)
        {
            var subAreas = await _areaRepository.GetSubAreas(areaId);

            if (subAreas == null)
            {
                return BadRequest("There was an error to load the subAreas.");
            }
            return Json(subAreas);
        }

        [HttpGet("{id}/alterar/autor")]
        public async Task<IActionResult> AlterarAutor(long id)
        {
            var autoresId = await _trabalhoRepository.GetAutoresId(id);

            AutoresViewModel model = new AutoresViewModel();

            List<AutorViewModel> coautores = new List<AutorViewModel>() { };

            foreach (var autor in autoresId)
            {
                var info = await _trabalhoRepository.GetAutor(autor.UsuarioId);

                var autorInfo = new AutorViewModel()
                {
                    Id = autor.UsuarioId,
                    Email = info.Email,
                    Nome = info.Nome.ToUpper(),
                    Orientador = autor.Orientador,
                    Sobrenome = info.Sobrenome.ToUpper(),
                    StatusId = autor.StatusUsuarioId
                };

                if (autorInfo.StatusId == 5)
                {
                    model.AutorPrincipal = autorInfo;
                }

                else if (autorInfo.Orientador == true)
                {
                    model.Orientador = autorInfo;
                }
                else
                {
                    coautores.Add(autorInfo);
                }
            }

            model.Coautores = coautores;

            return View("AlterarAutores", model);
        }

        [HttpPost("salva/autor")]
        public async Task<IActionResult> AlterarAutor(AutoresViewModel model) 
        {
            return Ok();
        }

        [HttpGet("pesquisa/autor")]
        public async Task<IActionResult> PesquisaAutor(string busca)
        {
            var autores = await _trabalhoRepository.PesquisaAutor(busca);
            List<AutorViewModel> autoresList = new List<AutorViewModel>();
            foreach (var autor in autores)
            {
                var instituicao = await _trabalhoRepository.GetInstituicao(autor.InstituicaoId);
                var status = await _trabalhoRepository.GetStatusAutor(autor.Id);
                var autorInfo = new AutorViewModel()
                {
                    Email = autor.Email,
                    Id = autor.Id,
                    Nome = autor.Nome,
                    Sobrenome = autor.Sobrenome,
                    Instituicao = instituicao,
                    StatusId = status,
                };
                autoresList.Add(autorInfo);
            }
            return Json(autoresList);
        }


        [HttpPost("busca/autor")]
        public async Task<IActionResult> AdicionaAutor(CadastroTrabalhoViewModel model)
        {
            return View();
        }

        public async Task<string> geraIdentificacao(Evento evento)
        {
            var rand = new Random();
            var next = rand.Next(10000, 99999);

            var identificacao = evento.Sigla + "2017" + next;

            if (await _trabalhoRepository.getIdentificacaoTrabalho(identificacao))
            {
                return identificacao;
            }
            else
            {
                return await geraIdentificacao(evento);
            }

        }


    }
}
