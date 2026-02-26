using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;

namespace WebApp.Application.Comisiones.Queries.GetComisionesActivas;

public class GetComisionActivaResponse
{
    public int id { get; set; }
    public string? Nombramiento { get; set; }
    public string? Departamento { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public string? Estado { get; set; }
    public bool Piloto { get; set; }
    public bool Prespuesto_Aprobado { get; set; }
    public List<GetComisionDestinosResponse>? destinos { get; set;}
}