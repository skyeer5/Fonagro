namespace WebApp.Domain.Gasolinas;
using WebApp.Domain.GasolinaPrecios;
using WebApp.Domain.Vehiculos;

public class Combustible 
{
    public int CombustibleId { get; set; }
    public string? Nombre { get; set; }
    public ICollection<Vehiculo>? Vehiculos { get; set; }
    public ICollection<CombustiblePrecio>? GasolinaPrecios { get; set; }

}