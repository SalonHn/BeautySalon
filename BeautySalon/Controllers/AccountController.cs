using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeautySalon.Models.DataBase;
using BeautySalon.Models.CreateModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using BeautySalon.Models;
using System.Timers;

namespace BeautySalon.Controllers
{
    public class AccountController : Controller
    {
        private readonly BeautysalonContext _context;
        private readonly Metodos _metodos = new Metodos();

        public AccountController(BeautysalonContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            UserAdmin? userAdmin = new UserAdmin();

            userAdmin = (from u in _context.UserAdmins 
                         where u.UserName == username && u.UserPassword == password
                         select u).FirstOrDefault();


            if(userAdmin != null)
            {
                if(userAdmin.IdType != 1 && userAdmin.UserActive == false)
                {
                    ViewBag.Error = "Usuario desactivado";
                    return View();
                }
                string userName = GetNombre(userAdmin.IdUser, userAdmin.IdType);
                TypeUser? typeUser = _context.TypeUsers.Find(userAdmin.IdType);
                var claim = new List<Claim> 
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim("idUser", userAdmin.IdUser.ToString())
                };

                if(typeUser != null )
                {
                    claim.Add(new Claim(ClaimTypes.Role, typeUser.TypeName));
                }

                if(userAdmin.IdType == 1 && userAdmin.UserActive == true)
                {
                    claim.Add(new Claim(ClaimTypes.Role, "VIP"));
                }

                var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                //_metodos.addBitacora(userAdmin.IdUser, 1, "Inicio de session", "");

                if(userAdmin.IdType == 1)
                {
                    return RedirectToAction("Index", "Clientes");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Credenciales invalidas";
            return View();
        }

        public string GetNombre(int id, int idType)
        {
            string nombre = "Sin Nombre";

            if(idType == 1)
            {
                Customer? customer = _context.Customers.Where(c=> c.IdUser == id).FirstOrDefault();
                if (customer != null) { nombre = customer.FullName; }
            }
            else
            {
                Employee? employee = _context.Employees.Where(e => e.IdUser == id).FirstOrDefault();
                if(employee != null) { nombre = employee.FirstName + " " + employee.LastName; }
            }

            return nombre;
        }

        public async Task<IActionResult> Logout()
        {
            int idUser = Int32.Parse(User.FindFirst("idUser").Value);
            //_metodos.addBitacora(idUser, 1, "Cerro session con exito", "");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        public IActionResult Registrarse()
        {
            List<string> genero = new List<string> {"Femenino", "Masculino" };
            ViewBag.Genero = genero;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse([Bind("FullName", "Gender", "Age", "Phone", "Email", "UserName", "UserPassword", "UserPasswordConfirm")] CreateCliente cliente)
        {
            if (!ModelState.IsValid)
            {
                List<string> genero = new List<string> { "Masculino", "Femenino" };
                ViewBag.Genero = genero;
                return View(cliente);
            }

            UserAdmin? userExist = _context.UserAdmins.Where(u=> u.UserName == cliente.UserName).FirstOrDefault();
            if(userExist != null)
            {
                List<string> genero = new List<string> { "Masculino", "Femenino" };
                ViewBag.Genero = genero;
                ViewBag.Existe = "El nombre de usuario ya existe";
                return View(cliente);
            }

            Customer? _cliente = _context.Customers.Where(c => c.Phone == cliente.Phone).FirstOrDefault();
            if(_cliente != null)
            {
                List<string> genero = new List<string> { "Masculino", "Femenino" };
                ViewBag.Genero = genero;
                ViewBag.Tel = "Ya existe un usuario asociado a este telefono.";
                return View(cliente);
            }

            if(cliente.UserPassword != cliente.UserPasswordConfirm)
            {
                List<string> genero = new List<string> { "Masculino", "Femenino" };
                ViewBag.Genero = genero;
                ViewBag.Pass = "Las contraseñas no coinciden.";
                return View(cliente);
            }

            DateTime fecha = DateTime.Now;

            //Creando usuario
            UserAdmin user = new UserAdmin
            {
                UserName = cliente.UserName,
                UserActive = false,
                UserPassword = cliente.UserPassword,
                UserDateCreate = fecha,
                UserDateModify = fecha,
                IdType = 1
            };
            var _user = _context.UserAdmins.Add(user);
            await _context.SaveChangesAsync();

            //Creando cliente
            Customer customer = new Customer
            {
                FullName = cliente.FullName,
                Phone = cliente.Phone,
                Gender = cliente.Gender,
                Age = cliente.Age,
                Email = cliente.Email,
                CreateDate = fecha,
                IdUser = _user.Entity.IdUser
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cliente.FullName),
                new Claim("idUser", cliente.IdUser.ToString())
            };
            claim.Add(new Claim(ClaimTypes.Role, "Cliente"));

            var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

            _metodos.addBitacora(cliente.IdUser, 1, "Registro de usuario", "Registro por primera ves de este usuario");

            return RedirectToAction("Index", "Clientes");
        }
    }
}
