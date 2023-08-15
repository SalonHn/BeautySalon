using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySalon.Models.DataBase;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UserController : Controller
    {
        private readonly BeautysalonContext _context;

        public UserController(BeautysalonContext context)
        {
            _context = context;
        }

        // GET: User
        public IActionResult Index()
        {
            var allUser = _context.Employees
                .Join(_context.UserAdmins,employee => employee.IdUser,user => user.IdUser,(employee, user) => new { employee, user })
                .Join(_context.TypeUsers, empleado=> empleado.user.IdType, type=> type.IdType, (empleado, type)=> new {empleado, type })
                .ToList();

            List<ViewModelAllUser> users = allUser.ConvertAll(x =>
                new ViewModelAllUser
                {
                    Id = x.empleado.user.IdUser,
                    FullName = x.empleado.employee.FirstName + " " + x.empleado.employee.LastName,
                    Name = x.empleado.employee.FirstName,
                    Type = x.type.TypeName,
                    userActive = x.empleado.user.UserActive,
                    genero = x.empleado.employee.Gender
                }
            );

            ViewBag.Users = users;

            return View();
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserAdmins == null)
            {
                return NotFound();
            }

            var userAdmin = await _context.UserAdmins
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (userAdmin == null)
            {
                return NotFound();
            }

            return View(userAdmin);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            List<RoleEmployee> skills = _context.RoleEmployees.Where(s=> s.IdRole != 1).ToList();
            List<TypeUser> roles = _context.TypeUsers.Where(r=> r.IdType != 1).ToList();
            List<string> genero = new List<string> { "Masculino", "Femenino" };
            ViewBag.Genero = genero;
            ViewBag.Skill = skills;
            ViewBag.Rol = roles;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("FirstName, LastName, Dni, Phone, DateOfBirth, Gender, Age, IdRole")] Employee empleado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    List<RoleEmployee> skills = _context.RoleEmployees.Where(s => s.IdRole != 1).ToList();
                    List<TypeUser> roles = _context.TypeUsers.Where(r => r.IdType != 1).ToList();
                    List<string> genero = new List<string> { "Masculino", "Femenino" };
                    ViewBag.Genero = genero;
                    ViewBag.Skill = skills;
                    ViewBag.Rol = roles;

                    return View(empleado);
                }
                return RedirectToAction("Index", "User");
            }
            catch (Exception e)
            {
                ViewBag.Erro = e.Message;
                List<RoleEmployee> skills = _context.RoleEmployees.Where(s => s.IdRole != 1).ToList();
                List<TypeUser> roles = _context.TypeUsers.Where(r => r.IdType != 1).ToList();
                List<string> genero = new List<string> { "Masculino", "Femenino" };
                ViewBag.Genero = genero;
                ViewBag.Skill = skills;
                ViewBag.Rol = roles;
                return View();
            }
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserAdmins == null)
            {
                return NotFound();
            }

            var userAdmin = await _context.UserAdmins.FindAsync(id);
            if (userAdmin == null)
            {
                return NotFound();
            }
            return View(userAdmin);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUser,UserName,UserPassword,UserActive,UserDateCreate,UserDateModify,UserEmail")] UserAdmin userAdmin)
        {
            if (id != userAdmin.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAdminExists(userAdmin.IdUser))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userAdmin);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserAdmins == null)
            {
                return NotFound();
            }

            var userAdmin = await _context.UserAdmins
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (userAdmin == null)
            {
                return NotFound();
            }

            return View(userAdmin);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserAdmins == null)
            {
                return Problem("Entity set 'BeautysalonContext.UserAdmins'  is null.");
            }
            var userAdmin = await _context.UserAdmins.FindAsync(id);
            if (userAdmin != null)
            {
                _context.UserAdmins.Remove(userAdmin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAdminExists(int id)
        {
          return (_context.UserAdmins?.Any(e => e.IdUser == id)).GetValueOrDefault();
        }
    }
}
