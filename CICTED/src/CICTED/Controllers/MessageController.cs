using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CICTED.Domain.ViewModels.Account;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CICTED.Controllers
{ [Route("message")]
    public class MessageController : Controller
    {
        public IActionResult Index(MessageViewModel model)
        {
            return View(model);
        }
    }
}