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

            return View();
        }
    }
}
