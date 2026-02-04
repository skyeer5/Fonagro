namespace WebApp.Domain;

public class GasolinaPrecio : AuditableEntity
{
    public int GasolinaPrecioId { get; set; }
    public Gasolina? Gasolina { get; set; }
    public int GasolinaId { get; set; }
    public decimal Precio { get; set; }
    public DateTime? Fecha { get; set; }
    
}