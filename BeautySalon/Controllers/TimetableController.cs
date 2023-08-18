using BeautySalon.Models;
using BeautySalon.Models.DataBase;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TimetableController : Controller
    {
        private readonly BeautysalonContext _context;
        private readonly Metodos _metodos = new Metodos();

        public TimetableController(BeautysalonContext context)
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
                (openTimetable, closeHour) => new {openTimetable, closeHour}).ToList();

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
            List<Holiday>? holidays = null;
            holidays = _context.Holidays.Where(h=> h.Date.Date >= DateTime.Now.Date).OrderBy(h=> h.Date).ToList();

            ViewBag.Holidays = holidays;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFeriado(DateTime fecha, String motivo)
        {
            //Guardar feriado
            Holiday? holiday = _context.Holidays.Where(h=> h.Date == fecha).FirstOrDefault();
            if (holiday != null) 
            {
                return RedirectToAction("Index");
            }

            Holiday newHoliday = new Holiday();
            newHoliday.Date = fecha;
            newHoliday.Reason = motivo;
            _context.Holidays.Add(newHoliday);
            await _context.SaveChangesAsync();

            int idUser = Int32.Parse(User.FindFirst("idUser").Value);
            _metodos.addBitacora(idUser, 1, "Nuevo feriado", "Se agrego un feriado para la fecha " + fecha.ToString("dd-MM-yyyy"));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DelFeriado(int id)
        {
            Holiday? holiday = _context.Holidays.Find(id);
            if (holiday != null) 
            {
                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 1, "Elimindado de feriado", "Se elimino el feriado de la fecha " + holiday.Date.ToString("dd-MM-yyyy"));

                _context.Holidays.Remove(holiday);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public IActionResult ConfigurarHorario()
        {
            List<Timetable> horarios = _context.Timetables.ToList();
            ViewModelAllHorario allHorarios = new ViewModelAllHorario();
            foreach (Timetable timetable in horarios)
            {
                if(timetable.IdTimetable == 1)
                {
                    allHorarios.domingo = timetable.IsHoliday;
                    allHorarios.domingoO = timetable.OpenHour;
                    allHorarios.domingoC = timetable.CloseHour;
                }
                if (timetable.IdTimetable == 2)
                {
                    allHorarios.lunes = timetable.IsHoliday;
                    allHorarios.lunesO = timetable.OpenHour;
                    allHorarios.lunesC = timetable.CloseHour;
                }
                if (timetable.IdTimetable == 3)
                {
                    allHorarios.martes = timetable.IsHoliday;
                    allHorarios.martesO = timetable.OpenHour;
                    allHorarios.martesC = timetable.CloseHour;
                }
                if (timetable.IdTimetable == 4)
                {
                    allHorarios.miercoles = timetable.IsHoliday;
                    allHorarios.miercolesO = timetable.OpenHour;
                    allHorarios.miercolesC = timetable.CloseHour;
                }
                if (timetable.IdTimetable == 5)
                {
                    allHorarios.jueves = timetable.IsHoliday;
                    allHorarios.juevesO = timetable.OpenHour;
                    allHorarios.juevesC = timetable.CloseHour;
                }
                if (timetable.IdTimetable == 6)
                {
                    allHorarios.viernes = timetable.IsHoliday;
                    allHorarios.viernesO = timetable.OpenHour;
                    allHorarios.viernesC = timetable.CloseHour;
                }
                if (timetable.IdTimetable == 7)
                {
                    allHorarios.sabado = timetable.IsHoliday;
                    allHorarios.sabadoO = timetable.OpenHour;
                    allHorarios.sabadoC = timetable.CloseHour;
                }
            } 

            ViewBag.Horas = _context.HoursAvailables.OrderBy(h=> h.IdHour).ToList();

            return View(allHorarios);
        }

        [HttpPost]
        public IActionResult ConfigurarHorario(ViewModelAllHorario allHorario)
        {

            Timetable? domingo = _context.Timetables.Find(1);
            if (domingo != null) 
            {
                domingo.IsHoliday = allHorario.domingo;
                domingo.OpenHour = allHorario.domingoO;
                domingo.CloseHour = allHorario.domingoC;
            }

            Timetable? lunes = _context.Timetables.Find(2);
            if (lunes != null)
            {
                lunes.IsHoliday = allHorario.lunes;
                lunes.OpenHour = allHorario.lunesO;
                lunes.CloseHour = allHorario.lunesC;
            }

            Timetable? martes = _context.Timetables.Find(3);
            if (martes != null)
            {
                martes.IsHoliday = allHorario.martes;
                martes.OpenHour = allHorario.martesO;
                martes.CloseHour = allHorario.martesC;
            }

            Timetable? miercoles = _context.Timetables.Find(4);
            if (miercoles != null)
            {
                miercoles.IsHoliday = allHorario.miercoles;
                miercoles.OpenHour = allHorario.miercolesO;
                miercoles.CloseHour = allHorario.miercolesC;
            }

            Timetable? jueves = _context.Timetables.Find(5);
            if (jueves != null)
            {
                jueves.IsHoliday = allHorario.jueves;
                jueves.OpenHour = allHorario.juevesO;
                jueves.CloseHour = allHorario.juevesC;
            }

            Timetable? viernes = _context.Timetables.Find(6);
            if (viernes != null)
            {
                viernes.IsHoliday = allHorario.viernes;
                viernes.OpenHour = allHorario.viernesO;
                viernes.CloseHour = allHorario.viernesC;
            }

            Timetable? sabado = _context.Timetables.Find(7);
            if (sabado != null)
            {
                sabado.IsHoliday = allHorario.sabado;
                sabado.OpenHour = allHorario.sabadoO;
                sabado.CloseHour = allHorario.sabadoC;
            }

            _context.SaveChanges();

            int idUser = Int32.Parse(User.FindFirst("idUser").Value);
            _metodos.addBitacora(idUser, 1, "Actualización de horario", "Se actualizo correctamente el horario");

            return RedirectToAction("Index");
        }
    }
}
