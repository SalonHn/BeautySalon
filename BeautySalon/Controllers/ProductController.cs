using BeautySalon.Models.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BeautySalon.Controllers
{
    public class ProductController : Controller
    {
        private readonly BeautysalonContext _context;

        public ProductController(BeautysalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Taxes = await _context.Taxes.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Sku", "NameProduct", "IdCategory", "ImgFile", "Description", "Price", "Stock", "IdTax", "StockMinimum")] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    ViewBag.Taxes = await _context.Taxes.ToListAsync();
                    return View(product);
                }

                Byte[] img;

                if(product.ImgFile != null)
                {
                    using (Stream fs = product.ImgFile.OpenReadStream())
                    {
                        using(BinaryReader br = new BinaryReader(fs))
                        {
                            img = br.ReadBytes((int)fs.Length);
                            product.ImageProduct = Convert.ToBase64String(img, 0, img.Length);
                        }
                    }
                }

                product.CreateDate = DateTime.Now;
                product.ModifyDate = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ViewBag.Taxes = await _context.Taxes.ToListAsync();
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
