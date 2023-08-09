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
    //[Authorize(Roles = "Usuarios")]
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
            var allUser = _context.Employees.Join(
                _context.UserAdmins, 
                employee => employee.IdUser, 
                user => user.IdUser, 
                (employee, user) => new { employee, user }).ToList();

            List<ViewModelAllUser> users = allUser.ConvertAll(x =>
                new ViewModelAllUser
                {
                    Id = x.user.IdUser,
                    name = x.employee.FirstName,
                    UserName = x.user.UserName,
                    userActive = x.user.UserActive,
                    genero = x.employee.Gender,
                    CreateDate = x.user.UserDateCreate
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
            List<RoleEmployee> roles = _context.RoleEmployees.ToList();
            List<string> genero = new List<string> { "Masculino", "Femenino", "Sin Especificar" };
            ViewBag.Genero = genero;
            ViewBag.Rol = roles;
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("FirstName, LastName, Dni, Phone, DateOfBirth, Gender, Age, IdRole, UserPasswordConfirm,UserName,UserPassword,UserEmail")] Employee empleado,
            bool citas,
            bool inventario,
            bool configuracion,
            bool servicios,
            bool reportes,
            bool usuarios)
        {
            if (!ModelState.IsValid)
            {
                List<RoleEmployee> roles = _context.RoleEmployees.ToList();
                List<string> genero = new List<string> { "Masculino", "Femenino", "Sin Especificar" };
                ViewBag.Genero = genero;
                ViewBag.Rol = roles;
                ViewBag.Error = "Aqui";

                return View(empleado);
            }

            //Creando usuario
            UserAdmin user = new UserAdmin();
            user.UserDateCreate = DateTime.Now;
            user.UserDateModify = DateTime.Now;
            user.UserActive = true;

            var userIngresado = _context.UserAdmins.Add(user);
            await _context.SaveChangesAsync();

            //Creando los permisos
            for (int i = 1; i <= 6; i++)
            {
                Permission permission = new Permission();
                permission.IdModule = i;
                permission.IdUser = userIngresado.Entity.IdUser;
                switch (i)
                {
                    case 1: permission.StatusPermission = usuarios; break;
                    case 2: permission.StatusPermission = citas; break;
                    case 3: permission.StatusPermission = inventario; break;
                    case 4: permission.StatusPermission = servicios; break;
                    case 5: permission.StatusPermission = configuracion; break;
                    case 6: permission.StatusPermission = reportes; break;
                }
                _context.Permissions.Add(permission);
                await _context.SaveChangesAsync();
            }

            // Guardando empleado
            empleado.IdUser = userIngresado.Entity.IdUser;
            empleado.DateCreate = DateTime.Now;
            empleado.DateModify = DateTime.Now;
            empleado.EmployeeActive = true;

            _context.Employees.Add(empleado);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
