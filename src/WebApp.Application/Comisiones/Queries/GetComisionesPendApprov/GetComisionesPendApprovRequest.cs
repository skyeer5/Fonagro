using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;

public class GetComisionesPendApprovRequest : PagingParameters
{
    public int Estado { get; set; }
    public DateTime? Fecha_Inicio { get; set; }
    public DateTime? Fecha_Fin { get; set; }
}