namespace WebApp.Application.Vehiculos.Queries.GetVehiculo;

public class GetVehiculoResponse
{
    public string? Placa { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? Tipo_Vehiculo { get; set; }
    public string? Color { get; set; }
    public int? Capacidad_Pasajeros { get; set; }
    public string? Tipo_Motor { get; set; }
    public double? Kilometraje { get; set; }   
}