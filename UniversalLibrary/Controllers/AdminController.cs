using Microsoft.AspNetCore.Mvc;

namespace UniversalLibrary.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
