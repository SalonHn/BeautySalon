using BeautySalon.Models.DataBase;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    public class TimetableController : Controller
    {
        private readonly BeautysalonContext _context;

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
            holidays = _context.Holidays.OrderBy(h => h.Date).ToList();

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

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DelFeriado(int id)
        {
            Holiday? holiday = _context.Holidays.Find(id);
            if (holiday != null) 
            {
                _context.Holidays.Remove(holiday);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ConfigurarFeriado()
        {
            return View();
        }
    }
}
