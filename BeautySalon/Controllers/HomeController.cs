using BeautySalon.Models;
using BeautySalon.Models.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BeautySalon.Controllers
{
    public class HomeController : Controller
    {
        private readonly BeautysalonContext _context;

        public HomeController(BeautysalonContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador,Estilista,Caja,Inventario")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}