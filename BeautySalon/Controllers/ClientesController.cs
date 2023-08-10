using BeautySalon.Models.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ClientesController : Controller
    {
        private readonly BeautysalonContext _context;

        public ClientesController(BeautysalonContext context)
        {
            _context = context;
        } 

        public IActionResult Index()
        {
            return View();
        }
    }
}
