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

        public IActionResult NuevaCita() 
        {
            if(User.FindFirst("idUser") != null)
            {
                int idCliente = Int32.Parse(User.FindFirst("idUser").Value);

                UserAdmin? cliente = _context.UserAdmins.Find(idCliente);
                
                if (cliente != null)
                {
                    ViewBag.Cliente = cliente;
                }
            }

            return View();
        }
    }
}
