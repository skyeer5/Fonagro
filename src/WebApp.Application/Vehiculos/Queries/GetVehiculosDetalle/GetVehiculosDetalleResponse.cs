namespace WebApp.Application.Vehiculos.Queries.GetVehiculosDetalle;

public class GetVehiculosDetalleResponse
{
    public int VehiculoId { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? Placa { get; set; }
    public string? Estado { get; set; }
}