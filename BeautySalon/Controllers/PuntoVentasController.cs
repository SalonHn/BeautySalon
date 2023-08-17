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

        [HttpPost]
        public async Task<IActionResult> Facturar([FromBody] CreateFactura factura)
        {
            try
            {
                Pago pago = new Pago();
                if (factura.IdTipoPago == 1)
                {
                    pago = new Pago
                    {

                    };
                }
                return Json(new { status = true });
            }
            catch
            {
                return Json(new {status = false});
            }
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
                int tipoTax = 1;
                if (product.IdTax == 1) { taxProduct = product.Price * 0.15m; }
                else { taxProduct = product.Price * 0.18m; tipoTax = 2; }
                encontrado = true;
                buscarProducto = new BuscarProducto
                {
                    idProducto = product.IdProduct,
                    name = product.NameProduct,
                    precio = product.Price,
                    tax = taxProduct,
                    cantidad = 1,
                    typeTax= tipoTax
                };
            }

            var respuesta = new
            {
                existe = encontrado,
                producto = buscarProducto
            };

            return Json(respuesta);
        }


        public JsonResult SearchCliente(string telefono)
        {
            int idCliente = 7;
            string nombreCliente = "Sin nombre";
            bool encontrado = false;

            Customer? customer = _context.Customers.Where(c=> c.Phone == telefono).FirstOrDefault();
            if(customer != null)
            {
                idCliente = customer.IdCustomer;
                nombreCliente = customer.FullName;
                encontrado = true;
            }

            var respuesta = new
            {
                existe = encontrado,
                id = idCliente,
                nombre = nombreCliente
            };

            return Json(respuesta);
        }
    }
}
