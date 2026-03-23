using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;

namespace WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;

public class GetComisionesPendApprovResponse
{
    public int id { get; set; }
    public string? Departamento { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public decimal Prespuesto_Estimado { get; set; }
}