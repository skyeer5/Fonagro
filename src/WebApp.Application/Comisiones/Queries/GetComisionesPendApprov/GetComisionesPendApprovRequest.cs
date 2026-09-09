using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;

public class GetComisionesPendApprovRequest : PagingParameters
{
    public int Estado { get; set; }
    public DateOnly? Fecha_Inicio { get; set; }
    public DateOnly? Fecha_Fin { get; set; }
}