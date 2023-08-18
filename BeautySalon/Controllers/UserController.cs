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
using BeautySalon.Models.CreateModels;
using BeautySalon.Models;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UserController : Controller
    {
        private readonly BeautysalonContext _context;
        private readonly Metodos _metodos = new Metodos();

        public UserController(BeautysalonContext context)
        {
            _context = context;
        }

        // GET: User
        public IActionResult Index()
        {
            var allUser = _context.Employees
                .Join(_context.UserAdmins, employee => employee.IdUser, user => user.IdUser, (employee, user) => new { employee, user })
                .Join(_context.TypeUsers, empleado => empleado.user.IdType, type => type.IdType, (empleado, type) => new { empleado, type })
                .ToList();

            List<ViewModelAllUser> users = allUser.ConvertAll(x =>
                new ViewModelAllUser
                {
                    Id = x.empleado.employee.IdEmployee,
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
        public IActionResult Details(int? id)
        {
            if (id == null || _context.UserAdmins == null)
            {
                return NotFound();
            }


            Employee? empleado = _context.Employees.Find(id);
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

            return View();
        }

        // GET: User/Create
        public IActionResult Create()
        {
            List<RoleEmployee> skills = _context.RoleEmployees.Where(s => s.IdRole != 1).ToList();
            List<TypeUser> roles = _context.TypeUsers.Where(r => r.IdType != 1).ToList();
            List<string> genero = new List<string> { "Masculino", "Femenino" };
            ViewBag.Genero = genero;
            ViewBag.Skill = skills;
            ViewBag.Rol = roles;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("FirstName", "LastName", "Dni", "Phone", "DateOfBirth", "Genero", "Age", "Email", "IdType", "IdRole", "UserName", "UserPassword", "UserPasswordConfirm")] CreateEmpleado empleado)
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

                if (empleado.UserPassword != empleado.UserPasswordConfirm)
                {
                    ViewBag.NoCorresponde = "Las contraseñas no coincien";
                    List<RoleEmployee> skills = _context.RoleEmployees.Where(s => s.IdRole != 1).ToList();
                    List<TypeUser> roles = _context.TypeUsers.Where(r => r.IdType != 1).ToList();
                    List<string> genero = new List<string> { "Masculino", "Femenino" };
                    ViewBag.Genero = genero;
                    ViewBag.Skill = skills;
                    ViewBag.Rol = roles;

                    return View(empleado);
                }

                DateTime fecha = DateTime.Now;

                //Guardar usuario
                UserAdmin user = new UserAdmin();
                user.UserName = empleado.UserName;
                user.UserPassword = empleado.UserPassword;
                user.IdType = empleado.IdType;
                user.UserActive = true;
                user.UserDateCreate = fecha;
                user.UserDateModify = fecha;
                var _user = _context.UserAdmins.Add(user);
                await _context.SaveChangesAsync();

                //Guardar empleado
                Employee employee = new Employee();
                employee.FirstName = empleado.FirstName;
                employee.LastName = empleado.LastName;
                employee.Email = empleado.Email;
                employee.Phone = empleado.Phone;
                employee.Gender = empleado.Genero;
                employee.Age = empleado.Age;
                employee.DateOfBirth = empleado.DateOfBirth;
                employee.Dni = empleado.Dni;
                employee.IdRole = empleado.IdRole;
                employee.IdUser = _user.Entity.IdUser;
                employee.DateCreate = fecha;
                employee.DateModify = fecha;
                employee.EmployeeActive = true;

                var _employee = _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 1, "Nuevo empleado", "Se agrego con exito el empleado " + empleado.FirstName);

                return RedirectToAction("Details", "User", new { id = _employee.Entity.IdEmployee});
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

                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 3, "Error al ingresar el empleado", "Se produjo el siguiente error: " + e.Message);

                return View();
            }
        }

        // GET: User/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CreateEmpleado empleado = new CreateEmpleado();
            Employee? _empleado = _context.Employees.Find(id);
            if (_empleado != null)
            {
                UserAdmin? user = _context.UserAdmins.Find(_empleado.IdUser);
                if(user != null)
                {
                    empleado = new CreateEmpleado
                    {
                        IdEmpleado = _empleado.IdEmployee,
                        IdUsuario = user.IdUser,
                        FirstName = _empleado.FirstName,
                        LastName = _empleado.LastName,
                        Dni = _empleado.Dni,
                        Genero = _empleado.Gender,
                        Email = _empleado.Email,
                        Phone = _empleado.Phone,
                        DateOfBirth = _empleado.DateOfBirth,
                        IdRole = _empleado.IdRole,
                        UserName = user.UserName,
                        IdType = user.IdType,
                        Age = _empleado.Age,
                        UserPassword = user.UserPassword,
                        UserPasswordConfirm = user.UserPassword
                    };
                }
            }
            
            List<RoleEmployee> skills = _context.RoleEmployees.Where(s => s.IdRole != 1).ToList();
            List<TypeUser> roles = _context.TypeUsers.Where(r => r.IdType != 1).ToList();
            List<string> genero = new List<string> { "Masculino", "Femenino" };
            ViewBag.Genero = genero;
            ViewBag.Skill = skills;
            ViewBag.Rol = roles;

            return View(empleado);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit
        (
            [Bind("FirstName", "LastName", "Dni", "Phone", "DateOfBirth", "Genero", "Age", "Email", "IdType", "IdRole", "UserName", "UserPassword", "UserPasswordConfirm", "IdEmpleado", "IdUsuario")] CreateEmpleado empleado
        )
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

                if (empleado.UserPassword != empleado.UserPasswordConfirm)
                {
                    ViewBag.NoCorresponde = "Las contraseñas no coincien";
                    List<RoleEmployee> skills = _context.RoleEmployees.Where(s => s.IdRole != 1).ToList();
                    List<TypeUser> roles = _context.TypeUsers.Where(r => r.IdType != 1).ToList();
                    List<string> genero = new List<string> { "Masculino", "Femenino" };
                    ViewBag.Genero = genero;
                    ViewBag.Skill = skills;
                    ViewBag.Rol = roles;

                    return View(empleado);
                }

                DateTime fecha = DateTime.Now;

                //Guardar usuario
                UserAdmin? user = _context.UserAdmins.Find(empleado.IdUsuario);
                if (user != null)
                {
                    user.UserName = empleado.UserName;
                    user.UserPassword = empleado.UserPassword;
                    user.IdType = empleado.IdType;
                    user.UserDateModify = fecha;

                    await _context.SaveChangesAsync();
                }

                //Guardar empleado
                Employee? employee = _context.Employees.Find(empleado.IdEmpleado);
                if (employee != null)
                {
                    employee.FirstName = empleado.FirstName;
                    employee.LastName = empleado.LastName;
                    employee.Email = empleado.Email;
                    employee.Phone = empleado.Phone;
                    employee.Gender = empleado.Genero;
                    employee.Age = empleado.Age;
                    employee.DateOfBirth = empleado.DateOfBirth;
                    employee.Dni = empleado.Dni;
                    employee.IdRole = empleado.IdRole;
                    employee.DateModify = fecha;

                    await _context.SaveChangesAsync();
                }

                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 1, "Empleado actualizado", "Se actualizo con exito el empleado " + empleado.FirstName);

                return RedirectToAction("Details", "User", new { id = empleado.IdEmpleado });
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

                int idUser = Int32.Parse(User.FindFirst("idUser").Value);
                _metodos.addBitacora(idUser, 3, "Error al actualizar el empleado", "Ocurrio el siguiente error: " + e.Message);

                return View();
            }
        }

        public async Task<IActionResult> CambioEstado(int idEmpleado, int idUser, bool estado)
        {
            Employee? empleado = _context.Employees.Find(idEmpleado);
            if(empleado != null)
            {
                empleado.EmployeeActive = estado;
                empleado.DateModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            UserAdmin? user = _context.UserAdmins.Find(idUser);
            if(user != null)
            {
                user.UserActive = estado;
                user.UserDateModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            int idU = Int32.Parse(User.FindFirst("idUser").Value);
            _metodos.addBitacora(idU, 1, "Estado de usuario actualizado", "El estado del usuario " + empleado.FirstName + " pasa a " + user.UserActive);

            return RedirectToAction("Details", "User", new { id = idEmpleado });
        }
    }
}
