using BeautySalon.Models.DataBase;

namespace BeautySalon.Models
{
    public class Metodos
    {
        private readonly BeautysalonContext _context;

        public Metodos(BeautysalonContext context)
        {
            _context = context;
        }

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
    }
}
