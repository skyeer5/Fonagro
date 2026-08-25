using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Comisiones.Queries.GetComisionesExcel;

namespace WebApp.Web.Models.Comisiones;

public class ComisionReportViewModel : GetComisionesExcelRequest
{
    public List<SelectListItem> DepartamentosList { get; set; } = [];
    public List<SelectListItem> VehiculosList { get; set; } = [];
    public List<SelectListItem> UsuariosList { get; set; } = [];
    public List<SelectListItem> EstadosList { get; set; } = [];
    public List<SelectListItem> UnidadesList { get; set; } = [];
}