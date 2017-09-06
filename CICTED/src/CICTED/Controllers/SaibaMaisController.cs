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

        [HttpGet("enkids")]
        public async Task<IActionResult> EnicKids()
        {
            return View();
        }

        [HttpGet("enteensjr")]
        public async Task<IActionResult> EnicTeensJr()
        {
            return View();
        }

        [HttpGet("mipg")]
        public async Task<IActionResult> Mipg()
        {
            return View();
        }

        [HttpGet("semex")]
        public async Task<IActionResult> Semex()
        {
            return View();
        }

        [HttpGet("seduni")]
        public async Task<IActionResult> Seduni()
        {
            return View();
        }

        [HttpGet("pp&i")]
        public async Task<IActionResult> PPEI()
        {
            return View();
        }

    }
}
