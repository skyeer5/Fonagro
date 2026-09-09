using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Domain.Comisiones;
using WebApp.Domain.Nombramientos;

namespace WebApp.Web.Models.Nombramientos;

public class NombramientoVerDetalleViewModel
{
    public int NombramientoId { get; set; }
    public DateOnly Fecha_Salida { get; set; }
    public DateOnly Fecha_Regreso { get; set; }

    public string Nombre_Completo { get; set; } = string.Empty;
    public string Puesto { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public string Correlativo { get; set; } = string.Empty;
    public string Proposito { get; set; } = string.Empty;
    public List<int> Municipios { get; set; } = [];
    public List<int> Departamentos { get; set; } = [];
    public NombramientoEstados NombramientoEstado { get; set; }
    public int? ComisionId { get; set; }
    public ComisionEstados? ComisionEstado { get; set; }
    public List<SelectListItem> DepartamentoList { get; set; } = [];
    public List<SelectListItem> MunicipioList { get; set; } = [];

}