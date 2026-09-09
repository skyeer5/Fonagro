using WebApp.Domain.Nombramientos;

namespace WebApp.Application.Nombramientos.Queries.GetNombramientos;

public class GetNombramientosResponse
{
    public int NombramientoId { get; set; }
    public string? Correlativo { get; set; }
    public string? Nombre_Nombrado { get; set; }
    public DateOnly Fecha_Salida { get; set; }
    public DateOnly Fecha_Regreso { get; set;}
    public List<string>? DepartamentosYMunicipios { get; set; }
    public NombramientoEstados Estado { get; set; }

}