using BeautySalon.Models;
using BeautySalon.Models.DataBase;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {

            UserAdmin? u = null;

            using (BeautysalonContext db = new BeautysalonContext())
            {
                u = (from b in db.UserAdmins
                     where b.UserName == "hola" && b.UserPassword == "hola"
                     select b).FirstOrDefault();
            }

            if(u == null)
            {
                ViewBag.hola = "Error";
            } else
            {
                ViewBag.hola = "sussesful";
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}