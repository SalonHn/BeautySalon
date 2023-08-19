using BeautySalon.Models.DataBase;
using BeautySalon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class DatosClienteController : Controller
    {
		private readonly BeautysalonContext _context;
        private readonly Metodos _metodos = new Metodos();

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

		public IActionResult MetodoPago()
		{
			int idCliente = Int32.Parse(User.FindFirst("idUser").Value);

			InformacionDePago? infoPago = _context.InformacionDePagos.Where(ip => ip.IdUser == idCliente).FirstOrDefault();
			ViewBag.InfoPagos = infoPago;

			return View();
		}


		[HttpPost]
		public IActionResult MetodoPago(string tarjeta, string vence, string ccv) 
		{
			try
			{
                int idCliente = Int32.Parse(User.FindFirst("idUser").Value);
                InformacionDePago info = new InformacionDePago
                {
                    IdUser = idCliente,
                    NumeroTarjeta = tarjeta,
                    Vence = vence,
                    Ccv = ccv
                };

				_context.InformacionDePagos.Add(info);
				_context.SaveChanges();

                _metodos.addBitacora(idCliente, 1, "Agrego metodo de pago", "");

                return RedirectToAction("MetodoPago", "DatosCliente");
            }
			catch (Exception ex)
			{
                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 3, "Error al agregar metodo de pago", "Ocurrio lo siguiente: " + ex.Message);
                return RedirectToAction("MetodoPago", "DatosCliente");
			}
		}


		public IActionResult removeTarjeta(int id)
		{
			try
			{
				InformacionDePago? info = _context.InformacionDePagos.Find(id);
				if(info != null)
				{
					_context.InformacionDePagos.Remove(info);
					_context.SaveChanges();

                    int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                    _metodos.addBitacora(idUser, 1, "Agrego metodo de pago", "");
                }

                return RedirectToAction("MetodoPago", "DatosCliente");
            }
			catch (Exception ex)
			{
                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 3, "Error al quitar metodo de pago", "Ocurrio lo siguiente: " + ex.Message);
                return RedirectToAction("MetodoPago", "DatosCliente");
            }
		}

		public IActionResult Membresia()
		{
            int idCliente = Int32.Parse(User.FindFirst("idUser").Value);

            InformacionDePago? infoPago = _context.InformacionDePagos.Where(ip => ip.IdUser == idCliente).FirstOrDefault();
            ViewBag.InfoPagos = infoPago;

            return View();
		}

		public IActionResult PagarMembresia()
		{
			try
			{
				int idUser = Int32.Parse(User.FindFirst("idUser").Value);

				return RedirectToAction("Index", "Clientes");
			}
			catch (Exception ex)
			{
				int idUser = Int32.Parse(User.FindFirst("idUser").Value);
				_metodos.addBitacora(idUser, 3, "Error obtener membresia", "Error: " + ex.Message);
				return RedirectToAction("Index", "Clientes");
			}
		}

    }
}
