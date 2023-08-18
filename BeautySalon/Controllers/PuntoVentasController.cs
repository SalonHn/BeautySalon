using BeautySalon.Models;
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
        private readonly Random _random = new Random();
        private readonly Metodos _metodos = new Metodos();

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
                Totales totales = CalcularTotal(factura.DetalleFactura);

                //Creacion de pago
                Pago pago = new Pago();
                if (factura.IdTipoPago == 1)
                {
                    pago = new Pago
                    {
                        Monto = totales.total,
                        Recibido = factura.Recibido,
                        Cambio = factura.Recibido - totales.total,
                        IdTipoPago = factura.IdTipoPago
                    };
                }
                else
                {
                    pago = pagoTarjeta(totales.total);
                }
                var _pago = _context.Pagos.Add(pago);
                _context.SaveChanges();

                //Creacion de factura

                int cant = _context.Invoices.Count() + 1000000;
                Invoice invoice = new Invoice
                {
                    ReferencesNumber = cant.ToString(),
                    IdCustomer = factura.IdCustomer,
                    NameCustomer = factura.NameCustomer,
                    DateInvoice = DateTime.Now,
                    Subtotal = totales.subtotal,
                    Tax = totales.isv,
                    Discount = 0,
                    Total = totales.total,
                    IdPago = _pago.Entity.IdPago
                };
                var _invoice = _context.Invoices.Add(invoice);
                _context.SaveChanges();

                //Agregando el detalle

                foreach(var item in factura.DetalleFactura)
                {
                    InvoiceDetail nuevoDetalle = new InvoiceDetail 
                    { 
                        IdProduct = item.idProducto,
                        IdInvoice = _invoice.Entity.IdInvoice,
                        IdTypeTax = item.typeTax,
                        Price = item.precio,
                        Tax = (double)(item.tax * item.cantidad),
                        Quantity = item.cantidad
                    };
                    _context.InvoiceDetails.Add(nuevoDetalle);
                }
                await _context.SaveChangesAsync();

                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 1, "Venta exitosa", "Se completo la venta exitosamente " + invoice.ReferencesNumber);

                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 3, "Venta fallida", "Se presento el siguiente error: " + ex.Message);

                return Json(new { status = false } );
            }
        }

        public Totales CalcularTotal(List<BuscarProducto> detalle) 
        {
            Totales resp = new Totales
            {
                subtotal = 0,
                isv = 0
            };
            foreach(var p in detalle)
            {
                resp.subtotal += (p.precio * p.cantidad);
                resp.isv += (p.tax * p.cantidad);
            }

            resp.total = resp.subtotal + resp.isv;

            return resp;
        }

        public Pago pagoTarjeta(decimal monto)
        {
            Pago pago = new Pago
            {
                IdTipoPago = 2,
                Monto = monto,
                NumeroDeTarjeta = "xxxx xxxx xxxx ",
                Ccv = "",
                Recibido = 0,
                Cambio = 0
            };

            for (int i = 0; i < 4; i++) {pago.NumeroDeTarjeta += _random.Next(0,10).ToString();}
            for (int i = 0; i < 3; i++) {pago.Ccv += _random.Next(0,10).ToString(); }
            int mes = _random.Next(1,13);
            if(mes < 10) { pago.Vence = "0" + mes.ToString() + "/23"; }
            else { pago.Vence = mes.ToString() + "/23"; }

            return pago;
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

        public IActionResult HistorialVentas(DateTime? fecha, string? cliente)
        {
            DateTime dateTime = fecha ?? DateTime.Now;
            List<Invoice> invoices = new List<Invoice>();
            if (fecha != null && cliente != null)
            {
                invoices = _context.Invoices.Where(i=> i.DateInvoice.Date == dateTime.Date && i.NameCustomer.Contains(cliente)).ToList();
            }else if(fecha != null && cliente == null)
            {
                invoices = _context.Invoices.Where(i=> i.DateInvoice.Date == dateTime.Date).ToList();
            }else if (cliente != null && fecha == null)
            {
                invoices = _context.Invoices.Where(i => i.NameCustomer.Contains(cliente)).ToList();
            } else
            {
                invoices = _context.Invoices.ToList();
            }

            ViewBag.Facturas = invoices;
            ViewBag.Cliente = cliente;
            if(fecha != null)
            {
                ViewBag.Fecha = dateTime.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.Fecha = fecha;
            }
            return View();
        }


        public IActionResult DetalleFactura(int idFactura)
        {
            Invoice? factura = _context.Invoices.Find(idFactura);
            ViewBag.Factura = factura;
            List<ViewDetalleFactura>? detalle = null;
            if(factura != null)
            {
                var detall = _context.InvoiceDetails
                    .Where(d => d.IdInvoice == factura.IdInvoice)
                    .Join(_context.Products, d=> d.IdProduct, p=> p.IdProduct, (d, p) => new {d, p})
                    .ToList();
                detalle = detall.ConvertAll(x=> new ViewDetalleFactura
                {
                    name = x.p.NameProduct,
                    cantidad = x.d.Quantity,
                    precio = x.d.Price
                });
            }
            ViewBag.Detalle = detalle;

            return View();
        }
    }
}
