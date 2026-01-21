namespace WebApp.Domain;

public class Gasolina : AuditableEntity
{
    public int GasolinaId { get; set; }
    public string? Nombre { get; set; }
    public ICollection<Vehiculo>? Vehiculos { get; set; }
    public ICollection<GasolinaPrecio>? GasolinaPrecios { get; set; }

}