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
                         where u.UserName == username && u.UserPassword == password && u.UserActive == true
                         select u).FirstOrDefault();

            if(userAdmin != null)
            {
                var claim = new List<Claim> 
                {
                    new Claim(ClaimTypes.Name, userAdmin.UserName),
                    new Claim("idUser", userAdmin.IdUser.ToString())
                };

                var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales invalidas";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Login();
        }

        public IActionResult LoginCustomer()
        {
            return View();
        }

        public IActionResult RegisterCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCustomer([Bind("IdCustomer","FullName", "Phone", "PinCustomer")] Customer customer, string pinConfirm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(customer);
                }

                if (pinConfirm != customer.PinCustomer)
                {
                    ViewBag.ErrorPin = "PINs don't match";
                    return View(customer);
                }
                Customer? existCustomer = null;
                existCustomer = (from c in _context.Customers
                                 where c.Phone == customer.Phone
                                 select c).FirstOrDefault();
                if (existCustomer != null)
                {
                    ViewBag.Error = "El numero de telefono ya esta registrado";
                    return View(existCustomer);
                }
                else
                {
                    customer.CreateDate = DateTime.Now;
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(LoginCustomer));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public IActionResult LogoutCustomer() 
        {
            return View();
        }
    }
}
