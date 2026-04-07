using WebApp.Application.Core;

namespace WebApp.Application.Vehiculos.Queries.GetVehiculosDetalle;

public class GetVehiculosDetalleRequest : PagingParameters
{
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? Placa { get; set; }
    public string? Estado { get; set; }
}