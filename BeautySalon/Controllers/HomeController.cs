using BeautySalon.Models;
using BeautySalon.Models.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BeautySalon.Controllers
{
    public class HomeController : Controller
    {
        private readonly BeautysalonContext _context;

        public HomeController(BeautysalonContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador,Estilista,Caja,Inventario")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Estilista,Caja,Inventario")]
        public IActionResult MyProfile() 
        {
            if (User.FindFirst("idUser") != null)
            {
                int id = Int32.Parse(User.FindFirst("idUser").Value);

                Employee? empleado = _context.Employees.Where(e=> e.IdUser == id).FirstOrDefault();
                if (empleado != null)
                {
                    ViewBag.Empleado = empleado;
                    UserAdmin? user = _context.UserAdmins.Find(empleado.IdUser);
                    ViewBag.User = user;
                    RoleEmployee? skill = _context.RoleEmployees.Find(empleado.IdRole);
                    ViewBag.Skill = skill;
                    if (user != null)
                    {
                        TypeUser? role = _context.TypeUsers.Find(user.IdType);
                        ViewBag.Role = role;
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}