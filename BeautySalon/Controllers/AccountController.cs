using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySalon.Models.DataBase;

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

        public IActionResult Logout()
        {
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
        public async Task<IActionResult> RegisterCustomer([Bind("FullName", "Phone", "PinCustomer")] Customer customer, int pinConfirm)
        {
            if (ModelState.IsValid)
            {
                if(pinConfirm != customer.PinCustomer)
                {
                    ViewBag.ErrorPin = "PINs don't match";
                    return View(customer);
                }
                else
                {
                    customer.CreateDate = DateTime.Now;
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(LoginCustomer));
                }
            }
            return View(customer);
        }

        public IActionResult LogoutCustomer() 
        {
            return View();
        }
    }
}
