using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Controllers
{
    [Route("saibaMais")]
    public class SaibaMaisController : Controller
    {
        [HttpGet("home")]
        public async Task<IActionResult> Home()
        {
            return View();
        }

        [HttpGet("enic")]
        public async Task<IActionResult> Enic()
        {
            return View();
        }
    }
}
