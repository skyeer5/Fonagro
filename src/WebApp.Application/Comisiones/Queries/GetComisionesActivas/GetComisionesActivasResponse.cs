using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;
using WebApp.Domain.Comisiones;

namespace WebApp.Application.Comisiones.Queries.GetComisionesActivas;

public class GetComisionActivaResponse
{
    public int ComisionId { get; set; }
    public string? Nombramiento { get; set; }
    public string? Departamento { get; set; }
    public string? Municipio { get; set; }
    public string? Descripcion { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public ComisionEstados Estado { get; set; }
    public bool Piloto { get; set; }
    public bool Prespuesto_Aprobado { get; set; }
    public List<GetComisionDestinosResponse>? Destinos { get; set;}
}