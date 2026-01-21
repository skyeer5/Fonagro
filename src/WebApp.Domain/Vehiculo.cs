namespace WebApp.Domain;

public class Vehiculo : AuditableEntity
{
    public int VehiculoId { get; set; }
    public string? Placa { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int? Anio { get; set; }
    public string? Tipo_Vehiculo { get; set; }
    public string? Color { get; set; }
    public int? Capacidad_Pasajeros { get; set; }
    public string? Tipo_Motor { get; set; }
    public double? Kilometraje { get; set; }
    
    public string? Estado { get; set; }
    public Gasolina? Gasolina { get; set; }
    public int? GasolinaId { get; set; }
    public ICollection<Accesorio>? Accesorios { get; set; }
    public ICollection<VehiculoAccesorio>? VehiculoAccesorios { get; set; }
    public ICollection<Parte>? Partes { get; set; }
    public ICollection<VehiculoParte>? VehiculoPartes { get; set; }
    public ICollection<Comision>? Comisiones { get; set; }
}