using WebApp.Domain.Comisiones;
using WebApp.Domain.Nombramientos;

namespace WebApp.Application.Nombramientos.Queries.GetNombramientoById;

public class GetNombramientoByIdResponse
{
    public int NombramientoId { get; set; }
    public string Nombre_Completo { get; set; } = string.Empty;
    public string Puesto { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public string Correlativo { get; set; } = string.Empty;
    public string Proposito { get; set; } = string.Empty;
    public List<int> Municipios { get; set; } = [];
    public List<int> Departamentos { get; set; } = [];
    public DateOnly Fecha_Salida { get; set; }
    public DateOnly Fecha_Regreso { get; set; }
    public NombramientoEstados NombramientoEstado { get; set; }
    public int? ComisionId { get; set; }
    public ComisionEstados? ComisionEstado { get; set; }
}