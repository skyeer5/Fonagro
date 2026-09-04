using WebApp.Application.Core;

namespace WebApp.Application.Nombramientos.Queries.GetNombramientos;

public class GetNombramientosRequest : PagingParameters
{
    public string? Nombre_Nombrado { get; set; }
    public int? Unidad { get; set; }
    public int? Correlativo { get; set; }
    public int? Estado { get; set; }
    public DateTime? Fecha_Inicio { get; set; }
    public DateTime? Fecha_Fin { get; set; }
}