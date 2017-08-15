using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CICTED.Domain.Infrastucture.Repository.Interfaces;
using CICTED.Domain.ViewModels.Autor;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("evento")]
    public class EventoController : Controller
    {
        private IEventoRepository _eventoRepository;

        public EventoController(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }       

        [HttpGet("descricao")]
        public async Task<IActionResult> Eventos(int id)
        {

            var eventos = await _eventoRepository.GetEvento(id);

            if (eventos == null) return BadRequest("There was an error to load the event.");

            HomeViewModel model = new HomeViewModel()
            {
                EventoNome = eventos.EventoNome,
                Descricao = eventos.Descricao,
                Objetivo = eventos.Objetivo,
                PublicoAlvo = eventos.PublicoAlvo
            };

            return Json(eventos);
        }
    }
}
