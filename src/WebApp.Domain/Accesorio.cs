namespace WebApp.Domain;

public class Accesorio : AuditableEntity
{
    public int AccesorioId { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
    public ICollection<Vehiculo>? Vehiculos { get; set; }
    public ICollection<VehiculoAccesorio>? VehiculoAccesorios { get; set; }
}