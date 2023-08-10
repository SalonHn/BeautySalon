using BeautySalon.Models.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = "Administrador,Inventario,Estilista")]
    public class ServiciosController : Controller
    {
        private readonly BeautysalonContext _context;

        public ServiciosController(BeautysalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
