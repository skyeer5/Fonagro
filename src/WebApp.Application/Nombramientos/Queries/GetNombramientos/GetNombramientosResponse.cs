using WebApp.Domain.Nombramientos;

namespace WebApp.Application.Nombramientos.Queries.GetNombramientos;

public class GetNombramientosResponse
{
    public int NombramientoId { get; set; }
    public string? Correlativo { get; set; }
    public string? Nombre_Nombrado { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set;}
    public string? Proposito { get; set; }
    public string? Nombre_Creador_Nombramiento { get; set; }
    public NombramientoEstados Estado { get; set; }

}