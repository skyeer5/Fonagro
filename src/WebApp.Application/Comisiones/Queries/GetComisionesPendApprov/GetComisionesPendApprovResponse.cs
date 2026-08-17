using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;

namespace WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;

public class GetComisionesPendApprovResponse
{
    public int ComisionId { get; set; }
    public string? Departamento { get; set; }
    public string? Municipio { get; set; }
    public List<string> Destinos { get; set; } = [];
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public decimal Kilometros { get; set; }
    public decimal Precio_Gasolina { get; set; }
    public decimal Prespuesto_Estimado { get; set; }
}