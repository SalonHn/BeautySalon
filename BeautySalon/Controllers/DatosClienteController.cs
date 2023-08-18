using BeautySalon.Models.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    public class DatosClienteController : Controller
    {
		private readonly BeautysalonContext _context;

		public DatosClienteController(BeautysalonContext context)
		{
			_context = context;
		} 

		public IActionResult MiPerfil()
        {
			int idCliente = Int32.Parse(User.FindFirst("idUser").Value);

			UserAdmin? user = _context.UserAdmins.Find(idCliente);
			Customer? cliente = _context.Customers.Where(c=> c.IdUser == idCliente).FirstOrDefault();
			ViewBag.Cliente = cliente;
			ViewBag.User = user;
			return View();
        }
    }
}
