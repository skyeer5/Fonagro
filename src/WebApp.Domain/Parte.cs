namespace WebApp.Domain;

public class Parte : AuditableEntity
{
    public int ParteId { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public ICollection<Vehiculo>? Vehiculos { get; set; }
    public ICollection<VehiculoParte>? VehiculoPartes { get; set; }
}