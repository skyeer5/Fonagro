using WebApp.Application.Core;
using WebApp.Domain.Comisiones;

namespace WebApp.Application.Comisiones.Queries.GetComisionesDetalle;

public class GetComisionesDetalleRequest : PagingParameters
{
    public DateOnly? Fecha_Inicio { get; set; }
    public DateOnly? Fecha_Fin { get; set; }
    public List<int>? Departamentos { get; set; }
    public List<int>? Municipios { get; set; }
    public int? Estado { get; set; }
}