using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySalon.Models.DataBase;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace BeautySalon.Controllers
{
    public class AccountController : Controller
    {
        private readonly BeautysalonContext _context;

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

                TypeUser? typeUser = _context.TypeUsers.Find(userAdmin.IdType);
                var claim = new List<Claim> 
                {
                    new Claim(ClaimTypes.Name, userAdmin.UserName),
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
