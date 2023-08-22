using BeautySalon.Models;
using BeautySalon.Models.DataBase;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ClientesController : Controller
    {
        private readonly BeautysalonContext _context;
        private readonly Metodos _metodos = new Metodos();

        public ClientesController(BeautysalonContext context)
        {
            _context = context;
        } 

        public IActionResult Index()
        {
            //Extrae el horario para mostrar
            var timetable = _context.Timetables.Where(timetable => timetable.IsHoliday == true).Join(
                _context.HoursAvailables,
                timetable => timetable.OpenHour,
                openHour => openHour.IdHour,
                (timetable, openHour) => new { timetable, openHour }).Join(
                _context.HoursAvailables,
                timetable => timetable.timetable.CloseHour,
                closeHour => closeHour.IdHour,
                (openTimetable, closeHour) => new { openTimetable, closeHour }).ToList();

            List<ViewModelHorario> horarios = timetable.ConvertAll(x =>
            new ViewModelHorario
            {
                Id = x.openTimetable.timetable.IdTimetable,
                day = x.openTimetable.timetable.Day,
                open = x.openTimetable.openHour.Hour,
                close = x.closeHour.Hour
            });

            ViewBag.Timetable = horarios;

            // Extrae los feriados para mostrar
            return View();
        }

        public IActionResult Servicios()
        {
            List<Product> servicios = _context.Products.Where(s => s.IdCategory == 1 && s.Featured==false).ToList();
            ViewBag.Servicios = servicios;

            if (User.IsInRole("VIP"))
            {
                List<Product> servicesVIP = _context.Products.Where(s => s.IdCategory == 1 && s.Featured == true).ToList();
                ViewBag.ServicesVIP = servicesVIP;
            }

            return View();
        }

        public IActionResult NuevaCita(int idServicio) 
        {
            if(User.FindFirst("idUser") != null)
            {
                int idCliente = Int32.Parse(User.FindFirst("idUser").Value);

                UserAdmin? cliente = _context.UserAdmins.Find(idCliente);
                
                if (cliente != null)
                {
                    ViewBag.Cliente = cliente;
                }

                Product? servicio = _context.Products.Find(idServicio);

                if (servicio != null)
                {
                    ViewBag.Servicio = servicio;
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult NuevaCita(int idServicio, int idCliente, int horaReserva, DateTime fechaReserva)
        {
            Reserva reserva = new Reserva();
            reserva.IdCustomer = idCliente;
            reserva.IdServicio = idServicio;
            reserva.Fecha = fechaReserva;
            reserva.IdHora = horaReserva;
            reserva.IdEstado = 1;

            _context.Reservas.Add(reserva);
            _context.SaveChanges();

            int idUser = Int32.Parse(User.FindFirst("idUser").Value);
            _metodos.addBitacora(idUser, 1, "Reserva de una cita", "Reservo una cita para el " + reserva.Fecha.ToString("dd-MM-yyyy"));

            return RedirectToAction("Reservas", "Clientes", new { estado = 1});
        }


        [HttpGet]
        public JsonResult ComprobarFecha(DateTime fecha)
        {
            int numeroDia = (int)fecha.DayOfWeek;
            bool estado = false;
            List<ViewModelHoras>? horas = null;

            Holiday? holiday = _context.Holidays
                .Where(h=> h.Date == fecha)
                .FirstOrDefault();

            if (holiday == null)
            {
                Timetable? horario = _context.Timetables.Find(numeroDia + 1);
                if (horario != null && horario.IsHoliday == true)
                {
                    var hours = _context.HoursAvailables
                        .Where(hour => hour.IdHour >= horario.OpenHour && hour.IdHour < horario.CloseHour)
                        .ToList();

                    horas = hours.ConvertAll(x=> new ViewModelHoras
                    {
                        Id = x.IdHour,
                        Hora = x.Hour
                    });

                    estado = true;
                }
            }

            var respuesta = new
            {
                laborable = estado,
                horario = horas
            };

            return Json(respuesta);
        }

        public IActionResult Reservas(int estado)
        {
            int idCliente = Int32.Parse(User.FindFirst("idUser").Value);

            var reservas = _context.Reservas
                .Where(reserva=> reserva.IdCustomer == idCliente && reserva.IdEstado == estado)
                .OrderBy(reserva=> reserva.Fecha)
                .Join(_context.Products, reserva=> reserva.IdServicio, servicio=> servicio.IdProduct, (reserva, servicio) => new {reserva, servicio})
                .Join(_context.HoursAvailables, reservas=> reservas.reserva.IdHora, hora=> hora.IdHour, (reservas, hora)=> new {reservas, hora})
                .ToList();

            List<ViewModelReserva> viewReservas = reservas.ConvertAll(x => new ViewModelReserva
            {
                Id = x.reservas.reserva.IdReserva,
                fecha = x.reservas.reserva.Fecha,
                Name = x.reservas.servicio.NameProduct,
                hora = x.hora.Hour,
                Vip = x.reservas.servicio.Featured
            });

            ViewBag.Reservas = viewReservas;

            List<EstadoReserva> estados = _context.EstadoReservas.ToList();
            ViewBag.Estados = estados;

            EstadoReserva? estadoReserva = _context.EstadoReservas.Find(estado);
            return View(estadoReserva);
        }


        public IActionResult DetallesReserva(int idReserva)
        {
            Reserva? reserva = _context.Reservas.Find(idReserva);

            if(reserva != null)
            {
                ViewBag.Reserva = reserva;

                Product? servcio = _context.Products.Find(reserva.IdServicio);
                ViewBag.Servicio = servcio;

                if(servcio != null)
                {
                    RoleEmployee? skill = _context.RoleEmployees.Find(servcio.IdSkill);
                    ViewBag.Skill = skill;
                }

                HoursAvailable? hora = _context.HoursAvailables.Find(reserva.IdHora);
                ViewBag.Hora = hora;

                EstadoReserva? estado = _context.EstadoReservas.Find(reserva.IdEstado);
                ViewBag.Estado = estado;

                return View();
            }
            return RedirectToAction("Index", "Clientes");
        }


        public IActionResult CancelarReserva(int idReserva)
        {
            Reserva? reserva = _context.Reservas.Find(idReserva);
            if(reserva != null)
            {
                reserva.IdEstado = 2;
                _context.SaveChanges();

                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 1, "Cancalacion de reserva", "Se cancelo la reserva programada para el " + reserva.Fecha.ToString("dd-MM-yyyy"));
            }

            return RedirectToAction("DetallesReserva", "Clientes", new { idReserva = idReserva });
        }

        public IActionResult Catalogo(int? categoria, int? page)
        {
            categoria = categoria ?? 0;
            int numPage = page ?? 1;
            List<Product> productos = new List<Product>();
            if(categoria == 0)
            {
                productos = _context.Products.Where(p=> p.IdCategory != 1).ToList();
            }
            else
            {
                productos = _context.Products.Where(p => p.IdCategory == categoria).ToList();
            }

            IPagedList<Product> pagedItems = productos.ToPagedList(numPage, 10);

            ViewBag.Productos = pagedItems;

            List<Category> categorias = _context.Categories.Where(c => c.IdCategory != 1).ToList();
            ViewBag.Categorias = categorias;
            ViewBag.IdCategoria = categoria;

            return View();
        }
    }
}
