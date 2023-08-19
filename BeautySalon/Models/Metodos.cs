using BeautySalon.Models.DataBase;

namespace BeautySalon.Models
{
    public class Metodos
    {
        private readonly BeautysalonContext _context = new BeautysalonContext();

        public async void addBitacora(int idUsuario, int nivel, string descripcion, string detalles)
        {
            Bitacora bitacora = new Bitacora
            {
                Nivel = nivel,
                Fecha = DateTime.Now,
                Usuario = idUsuario,
                Descripcion = descripcion,
                DetallesAdicionales = detalles
            };

            _context.Bitacoras.Add(bitacora);
            await _context.SaveChangesAsync();
        }

        public async void restarStock(int idProduct, int cantidad)
        {
            Product? product = _context.Products.Find(idProduct);
            if (product != null)
            {
                if(product.IdCategory != 1)
                {
                    if (product.Stock <= cantidad)
                    {
                        product.Stock = 0;
                    }
                    else
                    {
                        product.Stock -= cantidad;
                    }

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
