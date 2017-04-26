using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CICTED.Domain.Infrastucture.Services.Interfaces;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{
    [Route("localizacao")]
    public class LocalizacaoController : Controller
    {

        #region Construtor e Injeção
        private ILocalizacaoServices _localization;

        public LocalizacaoController(ILocalizacaoServices localization)
        {
            _localization = localization;
        }
        
        #endregion

        [HttpGet]
        [AllowAnonymous]
        [Route("lista/cidades/{estado}")]
        public async Task<IActionResult> ListaCidades(int estado)
            {
                var cidades = await _localization.GetCidades(estado);

            if (cidades == null) return BadRequest("There was an error to load the cities.");

            return Json(cidades);
        }
        
    }
}
