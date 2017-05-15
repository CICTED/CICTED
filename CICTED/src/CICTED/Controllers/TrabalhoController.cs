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
            CadastroTrabalhoViewModel model = new CadastroTrabalhoViewModel()
            {
                Evento = evento.EventoNome,
                AreasConhecimento = areas,
            };

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roles = await _accountRepository.GetRoles(user.Id);
            model.Roles = roles;
            ViewBag.Nome = user.Nome;


            return View(model);
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

        [HttpGet("informacao")]
        [Authorize]
        public async Task<IActionResult> Informacao(long id)
        {
            var trabalho = await _trabalhoRepository.GetInformacaoTrabalho(id);
            var evento = await _eventoRepository.GetEvento(trabalho.EventoId);
            var palavrasChave = await _trabalhoRepository.GetPalavrasChave(id);
            var area = await _areaRepository.GetArea(trabalho.SubAreaConhecimentoId);
            var subArea = await _areaRepository.GetSubArea(trabalho.SubAreaConhecimentoId);
            var statusTrabalho = await _trabalhoRepository.GetStatusTrabalho(trabalho.StatusTrabalhoId);

            var autoresId = await _trabalhoRepository.GetAutoresId(id);

            List<AutorViewModel> autoresInfo = new List<AutorViewModel>() { };

            foreach (var autor in autoresId)
            {
                var info = await _trabalhoRepository.GetAutor(autor.UsuarioId);
                var autorInfo = new AutorViewModel()
                {
                    Email = info.Email,
                    Nome = info.Nome.ToUpper(),
                    Orientador = autor.Orientador,
                    Sobrenome = info.Sobrenome.ToUpper(),
                    Status = autor.StatusUsuarioId
                };

                autoresInfo.Add(autorInfo);
            }

            


            var model = new InformacoesTrabalhoViewModel()
            {
                Id = id,
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
                AreaConhecimento = area,
                SubArea = subArea,
                Status = statusTrabalho,
                StatusTrabalhoId = trabalho.StatusTrabalhoId,
                autores = autoresInfo,
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

        [HttpGet("{id}/alterar")]
        public async Task<IActionResult> Alterar(long id)
        {
            return View("AlterarTrabalho");
        }

    }
}
