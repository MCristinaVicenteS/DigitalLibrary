using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UniversalLibrary.Models;

namespace UniversalLibrary.Controllers
{
    public class ErrorsController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]               //qd n conseguir encontrar a pág -> executa a iaction anterior e dp esta route
        public IActionResult Error404()
        {
            return View();
        }
    }
}
