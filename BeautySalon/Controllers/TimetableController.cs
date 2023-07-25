using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    public class TimetableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
