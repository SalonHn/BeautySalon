using BeautySalon.Models.DataBase;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = "Administrador,Inventario,Estilista")]
    public class ServiciosController : Controller
    {
        private readonly BeautysalonContext _context;

        public ServiciosController(BeautysalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Product> servicios = _context.Products.Where(p => p.IdCategory == 1 && p.Featured == false).ToList();

            List<Product> serviciosVIP = _context.Products.Where(p => p.IdCategory == 1 && p.Featured == true).ToList();

            ViewBag.Servicios = servicios;

            ViewBag.ServiciosVIP = serviciosVIP;

            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Skill = _context.RoleEmployees.Where(skill => skill.IdRole != 1).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Sku", "NameProduct", "Price", "ImgFile", "Featured", "Description", "IdSkill")] Product servicio)
        {
            servicio.IdCategory = 1;
            servicio.Stock = 0;
            servicio.StockMinimum = 0;
            servicio.IdTax = 1;

            if (!ModelState.IsValid)
            {
                ViewBag.Skill = _context.RoleEmployees.Where(skill => skill.IdRole != 1).ToList();
                return View(servicio);
            }

            Byte[] img;

            if (servicio.ImgFile != null)
            {
                using (Stream fs = servicio.ImgFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        img = br.ReadBytes((int)fs.Length);
                        servicio.ImageProduct = Convert.ToBase64String(img, 0, img.Length);
                    }
                }
            }

            servicio.CreateDate = DateTime.Now;
            servicio.ModifyDate = DateTime.Now;
            var servicioFinal = _context.Add(servicio);
            Product service = servicioFinal.Entity;
            await _context.SaveChangesAsync();

            return RedirectToAction("EditRecursos", new { id = service.IdProduct });
        }

        public IActionResult EditRecursos(int id)
        {
            Product? servicio = _context.Products.Find(id);

            if (servicio != null)
            {
                //Retornando el servicio
                ViewBag.service = servicio;

                //Retornando la habilidad requerida
                var skill = _context.RoleEmployees.Find(servicio.IdSkill);
                if (skill != null)
                {
                    ViewBag.skill = skill.NameRole;
                }

                //Retornando los recursos configurados
                var joinService = _context.ServiceDetails.Where(service => service.IdService == id)
                    .Join(_context.Products, service => service.IdProduct, product => product.IdProduct, (service, product) => new { service, product })
                    .ToList();

                List<ViewModelProduct> recursos = joinService.ConvertAll(
                    x => new ViewModelProduct
                    {
                        id = x.service.Id,
                        name = x.product.NameProduct,
                        stock = x.service.Quantity,
                        sku = x.product.Sku
                    });

                ViewBag.recursos = recursos;

                return View();
            }

            return RedirectToAction("Privacy", "Home");
        }

        [HttpPost]
        public IActionResult NuevoRecurso(int idService, int idProduct, double cantidad)
        {
            ServiceDetail? existe = _context.ServiceDetails.Where(sd=> sd.IdService == idService && sd.IdProduct == idProduct).FirstOrDefault();
            if (existe == null)
            {
                ServiceDetail serviceDetail = new ServiceDetail();
                serviceDetail.IdService = idService;
                serviceDetail.IdProduct = idProduct;
                serviceDetail.Quantity = cantidad;
                _context.ServiceDetails.Add(serviceDetail);
                _context.SaveChanges();
            }
            else
            {
                existe.Quantity = existe.Quantity + cantidad;
                _context.SaveChanges();
            }

            return RedirectToAction("EditRecursos", "Servicios", new { id = idService });
        }


        public IActionResult DeleteRecurso(int idService, int recurso)
        {
            ServiceDetail? serviceDetail = _context.ServiceDetails.Find(recurso);
            if (serviceDetail != null)
            {
                _context.ServiceDetails.Remove(serviceDetail);
                _context.SaveChanges();
            }
            return RedirectToAction("EditRecursos", "Servicios", new { id = idService });
        }


        public IActionResult EditServicio(int id) 
        {
            ViewBag.Skill = _context.RoleEmployees.Where(skill => skill.IdRole != 1).ToList();

            Product? servicio = _context.Products.Find(id);

            if(servicio != null)
            {
                return View(servicio);
            }

            return RedirectToAction("Index", "Servicios"); 
        }

        [HttpPost]
        public IActionResult EditServicio([Bind("Sku", "NameProduct", "Price", "ImgFile", "Featured", "Description", "IdSkill", "IdProduct")] Product servicio)
        {
            servicio.IdCategory = 1;
            servicio.Stock = 0;
            servicio.StockMinimum = 0;
            servicio.IdTax = 1;

            if (!ModelState.IsValid)
            {
                ViewBag.Skill = _context.RoleEmployees.Where(skill => skill.IdRole != 1).ToList();
                return View(servicio);
            }

            Product? editServicio = _context.Products.Find(servicio.IdProduct);

            if(editServicio != null)
            {
                Byte[] img;

                if (servicio.ImgFile != null)
                {
                    using (Stream fs = servicio.ImgFile.OpenReadStream())
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            img = br.ReadBytes((int)fs.Length);
                            editServicio.ImageProduct = Convert.ToBase64String(img, 0, img.Length);
                        }
                    }
                }

                editServicio.NameProduct = servicio.NameProduct;
                editServicio.IdSkill = servicio.IdSkill;
                editServicio.Description = servicio.Description;
                editServicio.Featured = servicio.Featured;
                editServicio.Price = servicio.Price;
                editServicio.ModifyDate = DateTime.Now;

                _context.SaveChanges();

                return RedirectToAction("EditRecursos", "Servicios", new { id = servicio.IdProduct });
            }

            return RedirectToAction("Index", "Servicios");
        }

        [HttpGet]
        public IActionResult BuscarProducto(string buscar)
        {
            var producto = _context.Products
                .Where(p=>p.IdCategory != 1 && (p.Sku.Contains(buscar) || p.NameProduct.Contains(buscar)))
                .Take(5).ToList();

            List<ViewModelRecurso> productos = producto.ConvertAll(
                x=> new ViewModelRecurso
                {
                    Id = x.IdProduct,
                    Name = x.NameProduct,
                    Sku = x.Sku
                }
            );

            return new JsonResult(productos);
        }
    }
}
