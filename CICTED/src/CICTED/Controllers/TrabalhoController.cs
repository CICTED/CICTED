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
using CICTED.Domain.Infrastucture.Services.Interfaces;
using CICTED.Domain.Infrastucture.Repository;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("trabalho")]
    public class TrabalhoController : Controller
    {
        private static string urlRoot = "http://localhost:54134";
        private UserManager<ApplicationUser> _userManager;
        private IAccountRepository _accountRepository;
        private ITrabalhoRepository _trabalhoRepository;
        private IEventoRepository _eventoRepository;
        private IAreaRepository _areaRepository;
        private IAutorRepository _autorRepository;
        private IEmailServices _emailServices;
        private IAgenciaRepository _agenciaRepository;
        private IAvaliacaoRepository _avaliacaoRepository;


        public TrabalhoController(IEmailServices emailServices, ITrabalhoRepository trabalhoRepository, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository, IEventoRepository eventoRepository, IAreaRepository areaRepository, IAutorRepository autorRepository, IAgenciaRepository agenciaRepository, IAvaliacaoRepository avaliacaoRepository)
        {
            _emailServices = emailServices;
            _trabalhoRepository = trabalhoRepository;
            _userManager = userManager;
            _accountRepository = accountRepository;
            _eventoRepository = eventoRepository;
            _areaRepository = areaRepository;
            _autorRepository = autorRepository;
            _agenciaRepository = agenciaRepository;
            _avaliacaoRepository = avaliacaoRepository;
        }

        [HttpGet("cadastro/{idEvento}")]
        [Authorize]
        public async Task<IActionResult> CadastroTrabalho(int idEvento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var evento = await _eventoRepository.GetEvento(idEvento);
            var areas = await _areaRepository.GetAreas();
            var periodos = await _trabalhoRepository.GetPeriodos();
            var agencias = await _agenciaRepository.GetAgencias();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            AutorViewModel autorPrincipal = new AutorViewModel()
            {
                Email = user.Email,
                Nome = user.Nome.ToUpper(),
                Sobrenome = user.Sobrenome.ToUpper(),
                Id = user.Id
            };

            CadastroTrabalhoViewModel model = new CadastroTrabalhoViewModel()
            {
                EventoNome = evento.EventoNome,
                EventoId = evento.Id,
                Evento = evento,
                AreasConhecimento = areas,
                Periodos = periodos,
                Agencias = agencias,
                AutorPrincipal = autorPrincipal,
            };

            model.EventoId = idEvento;

            var roles = await _accountRepository.GetRoles(user.Id);
            model.Roles = roles;
            ViewBag.AutorNome = user.Nome;


            return View(model);
        }

        [HttpPost("cadastro/{idEvento}")]
        public async Task<IActionResult> CadastroTrabalho(CadastroTrabalhoViewModel model, int idEvento)
        {
            model.EventoId = idEvento;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var coautores = new List<AutorViewModel>();
            AutorViewModel orientador = null;

            if (model.CoautoresId != null)
            {
                foreach (var autor in model.CoautoresId)
                {
                    var coautor = await _autorRepository.GetAutor(autor);
                    coautor.Orientador = false;
                    coautor.Id = autor;
                    coautor.AutorResponsavel = false;
                    if (coautor.Nome == null)
                    {
                        coautor.StatusId = 3;
                    }
                    else
                    {
                        coautor.StatusId = 2;
                    }
                    coautores.Add(coautor);
                }
            }

            if (model.OrientadorId != 0)
            {
                orientador = await _autorRepository.GetAutor(model.OrientadorId);
                orientador.Orientador = true;
                orientador.Id = model.OrientadorId;
                orientador.AutorResponsavel = false;
                if (orientador.Nome == null)
                {
                    orientador.StatusId = 3;
                }
                else
                {
                    orientador.StatusId = 2;
                }
            }


            #region ModelState.IsValid
            if (!ModelState.IsValid)
            {
                if (model.OrientadorId == 0)
                {
                    ViewBag.orientador = false;
                }
                var areas = await _areaRepository.GetAreas();
                var periodos = await _trabalhoRepository.GetPeriodos();
                var agencias = await _agenciaRepository.GetAgencias();


                AutorViewModel autorPrincipal = new AutorViewModel()
                {
                    Email = user.Email,
                    Nome = user.Nome.ToUpper(),
                    Sobrenome = user.Sobrenome.ToUpper(),
                    Id = user.Id
                };

                model.Orientador = orientador;
                model.Coautores = coautores;
                model.SubAreaNome = await _areaRepository.GetSubArea(model.SubAreaId);
                model.AutorPrincipal = autorPrincipal;
                model.Agencias = agencias;
                model.AreasConhecimento = areas;
                model.Periodos = periodos;
                ViewBag.erro = true;
                return View(model);
            }
            #endregion ModelState.IsValid

            //gera identificação
            model.Evento = await _eventoRepository.GetEvento(idEvento);
            string identificacao = await geraIdentificacao(model.Evento);

            model.DataCadastro = DateTime.Now;

            model.StatusTrabalhoId = 3;

            var trabalhoId = await _trabalhoRepository.InsertTrabalho(model.StatusTrabalhoId, model.Titulo, model.Introducao, model.Metodologia, model.Resultados, model.Resumo, model.Conclusao, model.Referencias, model.NomeEscola, model.TelefoneEscola, model.CidadeEscola, identificacao, model.DataCadastro, model.TextoCitacao, model.CodigoCEP, model.AgenciaId, model.Evento.Id, model.ArtigoId, model.SubAreaId, model.PeriodoApresentacao);

            var palavrasChave = await _trabalhoRepository.CadastraPalavrasChave(model.PalavraChave, trabalhoId);

            if (trabalhoId > 0)
            {
                var autorPrincipal = await _trabalhoRepository.CadastraAutorTrabalho(user.Id, 1, false, trabalhoId, true);

                if (idEvento == 2 || idEvento == 3 || idEvento == 4)
                {
                    var cadastraAlunos = await _trabalhoRepository.CadastrarAlunoTrabalho(trabalhoId, model.AlunosNome);
                    model.ReturnMenssagem = "Alterações salvas";
                    return RedirectToAction("ConsultaTrabalho");
                }
                else
                {
                    //SALVAR AUTOR TRABALHO
                    if (coautores != null)
                    {
                        foreach (var coautor in coautores)
                        {
                            var autorTrabalho = await _trabalhoRepository.CadastraAutorTrabalho(coautor.Id, coautor.StatusId, coautor.Orientador, trabalhoId, false);
                        }
                    }

                    if (orientador != null)
                    {
                        var autorTrabalho = await _trabalhoRepository.CadastraAutorTrabalho(orientador.Id, orientador.StatusId, orientador.Orientador, trabalhoId, false);
                    }

                    model.ReturnMenssagem = "Alterações salvas";
                    return RedirectToAction("ConsultaTrabalho");
                }
            }
            return BadRequest();
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

            List<AutorTrabalho> autoresId = await _autorRepository.GetAutoresId(id);
            List<AutorViewModel> autores = new List<AutorViewModel>() { };

            foreach (var autor in autoresId)
            {
                var info = await _autorRepository.GetAutor(autor.UsuarioId);
               
                var autorInfo = new AutorViewModel()
                {
                    Email = info.Email,
                    Id = autor.UsuarioId,
                    Nome = info.Nome,
                    Orientador = autor.Orientador,
                    Sobrenome = info.Sobrenome,
                    StatusId = autor.StatusUsuarioId,
                    AutorResponsavel = autor.AutorResponsavel,
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
                StatusTrabalhoId = trabalho.StatusTrabalhoId,
                
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

        [HttpPost("deletar/autor")]
        public async Task<IActionResult> DeletarAutorTrabalho(long userId, long idTrabalho)
        {
            var deletar = await _trabalhoRepository.DeletarAutorTrabalho(userId, idTrabalho);

            if (deletar == true)
            {
                return Ok();
            }
            return BadRequest("Não foi possível excluir");
        }

        [HttpGet("{id}/alterar/autor")]
        public async Task<IActionResult> AlterarAutor(long id)
        {
            var autoresId = await _autorRepository.GetAutoresId(id);

            AutoresViewModel model = new AutoresViewModel();
            model.Id = id;
            List<AutorViewModel> coautores = new List<AutorViewModel>() { };

            foreach (var autor in autoresId)
            {
                var info = await _autorRepository.GetAutor(autor.UsuarioId);

                var autorInfo = new AutorViewModel()
                {
                    Id = autor.UsuarioId,
                    Email = info.Email,
                    Orientador = autor.Orientador,
                    StatusId = autor.StatusUsuarioId
                };
                if (info.Nome != null)
                {
                    autorInfo.Nome = info.Nome.ToUpper();
                    autorInfo.Sobrenome = info.Sobrenome.ToUpper();
                }

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

        [HttpGet("pesquisa/autor")]
        public async Task<IActionResult> PesquisaAutor(string busca)
        {
            var autores = await _autorRepository.PesquisaAutor(busca);
            List<AutorViewModel> autoresList = new List<AutorViewModel>();
            foreach (var autor in autores)
            {
                var instituicao = await _trabalhoRepository.GetInstituicao(autor.InstituicaoId);
                var status = await _autorRepository.GetStatusAutor(autor.Id);
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

        [HttpPost("adicionar/autor")]
        public async Task<IActionResult> AdicionarAutor(string email, long id, bool orientador)
        {
            try
            {
                //verifica se esse email já esta cadastrado, devolve id do usuario
                var usuario = await _accountRepository.BuscaUsuario(email);
                if (usuario != null)
                {
                    //verificar se esse usuario ja esta cadastrado no trabalho
                    var existeCadastro = await _trabalhoRepository.VerificaCadastroTrabalho(id, usuario.Id);

                    if (existeCadastro == true)
                    {
                        return BadRequest("Usuario já cadastrado no trabalho");

                    }
                    else
                    {
                        var autor = new AutorTrabalho()
                        {

                            StatusUsuarioId = (usuario.Nome == null) ? 3 : 2,
                            Orientador = orientador,
                            TrabalhoId = id,
                            UsuarioId = usuario.Id
                        };

                        var cadastrar = await _trabalhoRepository.CadastraAutorTrabalho(autor.UsuarioId, autor.StatusUsuarioId, autor.Orientador, autor.TrabalhoId, false);

                        if (cadastrar == true)
                        {
                            var autorCadastrado = new AutorViewModel
                            {
                                Id = usuario.Id,
                                Email = email,
                                Nome = (usuario.Nome == null) ? "" : usuario.Nome.ToUpper(),
                                Sobrenome = (usuario.Sobrenome == null) ? "" : usuario.Sobrenome.ToUpper(),
                                StatusId = (usuario.Nome == null) ? 3 : 2,
                            };
                            return Json(autorCadastrado);
                        }
                        else
                        {
                            return BadRequest("Não foi possível cadastrar usuario");
                        }
                    }
                }
                else
                {
                    string caracteresPermitidos = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
                    char[] chars = new char[6];
                    Random rd = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        chars[i] = caracteresPermitidos[rd.Next(0, caracteresPermitidos.Length)];
                    }
                    var senha = new string(chars);

                    var user = new ApplicationUser()
                    {
                        Email = email,
                        UserName = email,
                        NormalizedEmail = email,
                        NormalizedUserName = email,
                        FirstAccess = true,
                        DataCadastro = DateTime.Now,
                        CursosId = 1,
                        InstituicaoId = 1,
                        EnderecoId = 1,
                    };

                    var result = await _userManager.CreateAsync(user, senha);

                    if (result.Succeeded)
                    {
                        try
                        {
                            await _userManager.AddToRoleAsync(user, "AUTOR");

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var callbackUrl = Url.Action(
                               "ConfirmEmail", "Account",
                               new { user = user.UserName, code = code });

                            var url = $"{urlRoot}{callbackUrl}";

                            //email
                            var emailConfirmation = await _emailServices.EnviarEmail(user.Email, url, senha);

                            var autor = new AutorTrabalho()
                            {
                                StatusUsuarioId = 3,
                                Orientador = orientador,
                                TrabalhoId = id,
                                UsuarioId = user.Id
                            };

                            //cadastra autor no trabalho
                            var cadastrar = await _trabalhoRepository.CadastraAutorTrabalho(autor.UsuarioId, autor.StatusUsuarioId, autor.Orientador, autor.TrabalhoId, false);

                            if (cadastrar == true)
                            {
                                var autorCadastrado = new AutorViewModel
                                {
                                    Id = user.Id,
                                    Email = email,
                                    StatusId = 3
                                };
                                return Json(autorCadastrado);
                            }
                            else
                            {
                                return BadRequest("Não foi possível cadastrar usuario");
                            }
                        }
                        catch (Exception ex)
                        {
                            await _userManager.DeleteAsync(user);

                            return BadRequest("Houve um erro ao cadastrar o usuário.");
                        }
                    }
                    return Ok();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("adicionar/email")]
        public async Task<IActionResult> AdicionarEmail(string email)
        {
            try
            {
                //verifica se esse email já esta cadastrado, devolve id do usuario
                var usuario = await _accountRepository.BuscaUsuario(email);
                if (usuario != null)
                {
                    var autorCadastrado = new AutorViewModel
                    {
                        Id = usuario.Id,
                        Email = email,
                        Nome = (usuario.Nome == null) ? "" : usuario.Nome.ToUpper(),
                        Sobrenome = (usuario.Sobrenome == null) ? "" : usuario.Sobrenome.ToUpper(),
                        StatusId = (usuario.Nome == null) ? 3 : 2,
                    };
                    return Json(autorCadastrado);
                }
                else
                {
                    string caracteresPermitidos = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
                    char[] chars = new char[6];
                    Random rd = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        chars[i] = caracteresPermitidos[rd.Next(0, caracteresPermitidos.Length)];
                    }
                    var senha = new string(chars);

                    var user = new ApplicationUser()
                    {
                        Email = email,
                        UserName = email,
                        NormalizedEmail = email,
                        NormalizedUserName = email,
                        FirstAccess = true,
                        DataCadastro = DateTime.Now,
                        CursosId = 1,
                        InstituicaoId = 1,
                        EnderecoId = 1,
                    };

                    var result = await _userManager.CreateAsync(user, senha);

                    if (result.Succeeded)
                    {
                        try
                        {
                            await _userManager.AddToRoleAsync(user, "AUTOR");

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var callbackUrl = Url.Action(
                               "ConfirmEmail", "Account",
                               new { user = user.UserName, code = code });

                            var url = $"{urlRoot}{callbackUrl}";

                            //email
                            var emailConfirmation = await _emailServices.EnviarEmail(user.Email, url, senha);


                            var autorCadastrado = new AutorViewModel
                            {
                                Id = user.Id,
                                Email = email,
                                Nome = (usuario.Nome == null) ? "" : usuario.Nome.ToUpper(),
                                Sobrenome = (usuario.Sobrenome == null) ? "" : usuario.Sobrenome.ToUpper(),
                                StatusId = (usuario.Nome == null) ? 3 : 2,
                            };
                            return Json(autorCadastrado);
                        }
                        catch (Exception ex)
                        {
                            await _userManager.DeleteAsync(user);

                            return BadRequest("Houve um erro ao cadastrar o usuário.");
                        }
                    }
                    return Ok();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("avaliacao/painel")]
        public async Task<IActionResult> AvaliacaoPainel()
        {
            AvaliacaoTrabalhoViewModel model = new AvaliacaoTrabalhoViewModel();

            model.Eventos = await _eventoRepository.getEventos();


            return View(model);
        }

        [HttpPost("avaliacao/painel")]
        public async Task<IActionResult> AvaliacaoPainel(AvaliacaoTrabalhoViewModel model)
        {
            var trabalho = await _trabalhoRepository.GetTrabalho(model.IdentificadorTrabalho);

            

            var data = DateTime.Now;           
        
            if (await _avaliacaoRepository.InsertAvaliacao(trabalho.StatusTrabalhoId, model.AvaliadorId,trabalho.Id,model.Nota, model.NotaResumo, model.NotaMetodologia, model.NotaResultado, model.NotaObjetivo, model.Favorito,model.Comentario,data,2))
            {
                
                return RedirectToAction("avaliacaoPainel");
            }
            else
            {
                return BadRequest("Erro ao salvar avaliação");
            }
           
        }
        [HttpGet("pesquisa/trabalho")]
        public async Task<IActionResult> PesquisaTrabalho(string identificacao)
        {

            var trabalho = await _trabalhoRepository.GetTrabalho(identificacao);
            var subArea = await _trabalhoRepository.GetSubArea(trabalho.SubAreaConhecimentoId);
            var trabalhoInfo = new AvaliacaoTrabalhoViewModel();

            trabalhoInfo.TituloTrabalho = trabalho.Titulo;
            trabalhoInfo.SubArea = subArea.Nome;

            return Json(trabalhoInfo); 

        }
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

                return View(model);
            }
        }

        //public async Task<IActionResult> GraficoTrabalhos(int idEvento)
        //{

        //}

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
