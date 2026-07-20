namespace WebApp.Domain;
using WebApp.Domain.Gasolinas;

public class GasolinaPrecio : AuditableEntity
{
    public int GasolinaPrecioId { get; set; }
    public Gasolina? Gasolina { get; set; }
    public int GasolinaId { get; set; }
    public decimal Precio { get; set; }
    public DateTime? Fecha { get; set; }

    public static GasolinaPrecio Crear(int gasolinaId, decimal precio, int userId)
    {
        return new GasolinaPrecio
        {
            GasolinaId = gasolinaId,
            Precio = precio,
            Fecha = DateTime.Now,
            Creado_Por = userId
        };
    }
    
}