using WebApp.Application.Core;

namespace WebApp.Application.Nombramientos.Queries.GetNombramientos;

public class GetNombramientosRequest : PagingParameters
{
    public int? Usuario { get; set; }
    public int? Unidad { get; set; }
    public int? Correlativo { get; set; }
    public int? Estado { get; set; }
    public DateOnly? Fecha_Inicio { get; set; }
    public DateOnly? Fecha_Fin { get; set; }
}