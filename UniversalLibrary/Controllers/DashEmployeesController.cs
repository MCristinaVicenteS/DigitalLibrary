using Microsoft.AspNetCore.Mvc;

namespace UniversalLibrary.Controllers
{
    public class DashEmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
