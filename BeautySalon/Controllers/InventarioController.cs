using BeautySalon.Models.DataBase;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeautySalon.Controllers
{
    //[Authorize(Roles = "Administrador,Inventario")]
    public class InventarioController : Controller
    {
        private readonly BeautysalonContext _context;

        public InventarioController(BeautysalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var joinProductCategory = _context.Products.Join(_context.Categories,
                product => product.IdCategory,
                category => category.IdCategory,
                (product, category) => new { product, category }).ToList();

            List<ViewModelProduct> products = joinProductCategory.ConvertAll(x =>
                new ViewModelProduct
                {
                    id = x.product.IdProduct,
                    name = x.product.NameProduct,
                    sku = x.product.Sku,
                    price = x.product.Price,
                    stock = x.product.Stock,
                    categoria = x.category.Category1
                });

            return View(products);
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

                if (product.ImgFile != null)
                {
                    using (Stream fs = product.ImgFile.OpenReadStream())
                    {
                        using (BinaryReader br = new BinaryReader(fs))
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

        public IActionResult Detalles(int id)
        {
            Product? product = _context.Products.Find(id);
            Category? category = null;
            if (product != null)
            {
                category = _context.Categories.Find(product.IdCategory);
            }

            if (category != null && product != null)
            {
                ViewBag.Category = category;
                ViewBag.Product = product;
            }

            return View();
        }

        public async Task<IActionResult> Editar(int id)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Taxes = await _context.Taxes.ToListAsync();
            Product? product = _context.Products.Find(id);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Editar([Bind("IdProduct", "Sku", "NameProduct", "IdCategory", "ImgFile", "Description", "Price", "Stock", "IdTax", "StockMinimum")] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    ViewBag.Taxes = await _context.Taxes.ToListAsync();
                    return View(product);
                }

                var productOriginal = await _context.Products.FindAsync(product.IdProduct);

                if (productOriginal != null)
                {
                    Byte[] img;
                    //Verificando si hubo cambio de imagen
                    if (product.ImgFile != null)
                    {
                        using (Stream fs = product.ImgFile.OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                img = br.ReadBytes((int)fs.Length);
                                productOriginal.ImageProduct = Convert.ToBase64String(img, 0, img.Length);
                            }
                        }
                    }
                    //Agregando cambios
                    productOriginal.NameProduct = product.NameProduct;
                    productOriginal.Description = product.Description;
                    productOriginal.Price = product.Price;
                    productOriginal.IdCategory = product.IdCategory;
                    productOriginal.IdTax = product.IdTax;
                    productOriginal.Stock = product.Stock;
                    productOriginal.StockMinimum = product.StockMinimum;
                    productOriginal.Featured = product.Featured;
                    productOriginal.ModifyDate = DateTime.Now;
                    productOriginal.Sku = product.Sku;

                    //Actualizando
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ViewBag.Taxes = await _context.Taxes.ToListAsync();
                ViewBag.Error = ex.Message;
                return View(product);
            }
        }
    }
}
