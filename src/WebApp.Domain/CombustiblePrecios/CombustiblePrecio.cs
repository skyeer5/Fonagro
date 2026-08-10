namespace WebApp.Domain.GasolinaPrecios;
using WebApp.Domain.Gasolinas;

public class CombustiblePrecio : AuditableEntity
{
    public int CombustiblePrecioId { get; set; }
    public Combustible? Combustible { get; set; }
    public int CombustibleId { get; set; }
    public decimal Precio { get; set; }
    public DateTime? Fecha { get; set; }

    public static CombustiblePrecio Crear(int combustibleId, decimal precio, int userId)
    {
        return new CombustiblePrecio
        {
            CombustibleId = combustibleId,
            Precio = precio,
            Fecha = DateTime.Now,
            CreatedBy = userId
        };
    }
    
}