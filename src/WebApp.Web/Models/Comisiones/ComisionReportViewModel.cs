using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Comisiones.Queries.GetComisionesExcel;

namespace WebApp.Web.Models.Comisiones;

public class ComisionReportViewModel
{
    public List<SelectListItem> DepartamentosList { get; set; } = [];
    public List<SelectListItem> VehiculosList { get; set; } = [];
    public List<SelectListItem> UsuariosList { get; set; } = [];
    public List<SelectListItem> EstadosList { get; set; } = [];
    public List<SelectListItem> UnidadesList { get; set; } = [];
    public DateOnly? Fecha_Salida { get; set; }
    public DateOnly? Fecha_Regreso { get; set; }
    public List<int>? Departamentos { get; set; }
    public List<int>? Municipios { get; set; }
    public int? Vehiculo { get; set; }
    public int? Usuario { get; set; }
    public List<int>? Unidades { get; set; }
    public int? Estado { get; set; }
}