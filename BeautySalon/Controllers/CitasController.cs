using BeautySalon.Models;
using BeautySalon.Models.DataBase;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = "Administrador,Estilista,Caja")]
    public class CitasController : Controller
    {
        private readonly BeautysalonContext _context;
        private readonly Metodos _metodos = new Metodos();
        private readonly Random _random = new Random();

        public CitasController(BeautysalonContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? estado)
        {
            estado = estado ?? 1;
            List<ViewReservas> reservas = (from r in _context.Reservas
                            join s in _context.Products on r.IdServicio equals s.IdProduct
                            join h in _context.HoursAvailables on r.IdHora equals h.IdHour
                            join u in _context.UserAdmins on r.IdCustomer equals u.IdUser 
                            where r.IdEstado == estado
                            orderby r.Fecha
                            select new ViewReservas
                            {
                                id = r.IdReserva,
                                nameServicio = s.NameProduct,
                                user = u.UserName,
                                hora = h.Hour,
                                vip = s.Featured,
                                fecha = r.Fecha
                            }).ToList();

            ViewBag.Reservas = reservas;

            List<EstadoReserva> estados = _context.EstadoReservas.ToList();
            ViewBag.Estados = estados;

            EstadoReserva? estadoReserva = _context.EstadoReservas.Find(estado);
            return View(estadoReserva);
        }

        public IActionResult Detalles(int idReserva)
        {
            Reserva? reserva = _context.Reservas.Find(idReserva);

            if (reserva != null)
            {
                ViewBag.Reserva = reserva;

                Product? servcio = _context.Products.Find(reserva.IdServicio);
                ViewBag.Servicio = servcio;

                if (servcio != null)
                {
                    RoleEmployee? skill = _context.RoleEmployees.Find(servcio.IdSkill);
                    ViewBag.Skill = skill;
                }

                HoursAvailable? hora = _context.HoursAvailables.Find(reserva.IdHora);
                ViewBag.Hora = hora;

                EstadoReserva? estado = _context.EstadoReservas.Find(reserva.IdEstado);
                ViewBag.Estado = estado;

                UserAdmin? user = _context.UserAdmins.Find(reserva.IdCustomer);
                if (user != null)
                {
                    Customer? cliente = _context.Customers.Where(c => c.IdUser == user.IdUser).FirstOrDefault();
                    ViewBag.Cliente = cliente;
                }

                return View();
            }
            return RedirectToAction("Index", "Citas");
        }

        public IActionResult CancelarReserva(int idReserva)
        {
            Reserva? reserva = _context.Reservas.Find(idReserva);
            if (reserva != null)
            {
                reserva.IdEstado = 2;
                _context.SaveChanges();

                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 1, "Cancalacion de reserva", "Se cancelo la reserva programada para el " + reserva.Fecha.ToString("dd-MM-yyyy"));
            }

            return RedirectToAction("Detalles", "Citas", new { idReserva = idReserva });
        }

        public IActionResult ConfirmarPago(int idReserva, int tipoPago, decimal recibido)
        {
            try
            {
                int idUser = Int32.Parse(User.FindFirst("idUser").Value);

                Reserva? reserva = _context.Reservas.Find(idReserva);
                reserva.IdEstado = 3;
                Product? servicio = _context.Products.Find(reserva.IdServicio);

                Pago pago = new Pago();
                //Agregando el pago
                if(tipoPago == 1)
                {
                    pago = new Pago
                    {
                        IdTipoPago = 1,
                        Monto = servicio.Price * 1.15m,
                        Recibido = recibido,
                        Cambio = recibido - (servicio.Price * 1.25m)
                    };
                }
                else
                {
                    pago = pagoTarjeta(servicio.Price * 1.15m);
                }

                var _pago = _context.Pagos.Add(pago);
                _context.SaveChanges();

                //Creando la factura 
                string nReferencia = (_context.Invoices.Count() + 1000000).ToString();
                Customer? cliente = _context.Customers.Where(c => c.IdUser == reserva.IdCustomer).FirstOrDefault();
                Invoice factura = new Invoice
                {
                    ReferencesNumber = nReferencia,
                    IdCustomer = cliente.IdCustomer,
                    NameCustomer = cliente.FullName,
                    IdPago = _pago.Entity.IdPago,
                    DateInvoice = DateTime.Now,
                    Subtotal = servicio.Price,
                    Tax = servicio.Price * 0.15m,
                    Total = servicio.Price * 1.15m,
                    Discount = 0
                };
                var _factura = _context.Invoices.Add(factura);
                _context.SaveChanges();

                //Agregando el datalle
                InvoiceDetail detalle = new InvoiceDetail
                {
                    IdInvoice = _factura.Entity.IdInvoice,
                    IdProduct = servicio.IdProduct,
                    Quantity = 1,
                    Price = servicio.Price,
                    Tax = (double)(servicio.Price * 0.15m),
                    IdTypeTax = 1
                };
                _context.InvoiceDetails.Add(detalle);
                _context.SaveChanges();

                _metodos.addBitacora(idUser, 1, "Pago de reserva", "Se pago la reserva con id " + reserva.IdReserva);

                return View();
            }
            catch (Exception ex)
            {
                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 3, "Error al pagar reserva", "Error: " + ex.Message);
                return RedirectToAction("Index", "Citas");
            }
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

            for (int i = 0; i < 4; i++) { pago.NumeroDeTarjeta += _random.Next(0, 10).ToString(); }
            for (int i = 0; i < 3; i++) { pago.Ccv += _random.Next(0, 10).ToString(); }
            int mes = _random.Next(1, 13);
            if (mes < 10) { pago.Vence = "0" + mes.ToString() + "/24"; }
            else { pago.Vence = mes.ToString() + "/24"; }

            return pago;
        }
    }
}
