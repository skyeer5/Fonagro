using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Queries.GetComisionesDetalle;

public class GetComisionesDetalleRequest : PagingParameters
{
    public DateTime? Fecha_Inicio { get; set; }
    public DateTime? Fecha_Fin { get; set; }
    public string? Departamento { get; set; }
}