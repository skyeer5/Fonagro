namespace WebApp.Domain.Gasolinas;
using WebApp.Domain.GasolinaPrecios;
using WebApp.Domain.Vehiculos;

public class Gasolina 
{
    public int GasolinaId { get; set; }
    public string? Nombre { get; set; }
    public ICollection<Vehiculo>? Vehiculos { get; set; }
    public ICollection<GasolinaPrecio>? GasolinaPrecios { get; set; }

}