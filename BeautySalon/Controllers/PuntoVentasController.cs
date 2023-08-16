using BeautySalon.Models.CreateModels;
using BeautySalon.Models.DataBase;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = "Administrador,Caja")]
    public class PuntoVentasController : Controller
    {
        private readonly BeautysalonContext _context;

        public PuntoVentasController(BeautysalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult NuevaVenta()
        {
            return View();
        }


        //Obtener un Servicio o Producto
        public JsonResult SearchProducto(string sku)
        {
            bool encontrado = false;
            BuscarProducto buscarProducto = new BuscarProducto();

            Product? product = _context.Products.Where(p=> p.Sku == sku).FirstOrDefault();
            if (product != null)
            {
                decimal taxProduct = 0;
                if (product.IdTax == 1) { taxProduct = product.Price * 0.15m; }
                else { taxProduct = product.Price * 0.18m; }
                encontrado = true;
                buscarProducto = new BuscarProducto
                {
                    idProducto = product.IdProduct,
                    name = product.NameProduct,
                    precio = product.Price,
                    tax = taxProduct,
                    cantidad = 1
                };
            }

            var respuesta = new
            {
                existe = encontrado,
                producto = buscarProducto
            };

            return Json(respuesta);
        }
    }
}
