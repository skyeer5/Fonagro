using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Comisiones.Queries.GetComisionesDetalle;
using WebApp.Application.Core;

namespace WebApp.Web.Models.Comisiones;

public class ComisionListViewModel
{
    public List<GetComisionesDetalleResponse> ComisionesList { get; set; } = [];
    public List<SelectListItem> DepartamentosList { get; set; } = [];
    public List<SelectListItem> EstadosList { get; set; } = [];
    public DateOnly? Fecha_Inicio { get; set; }
    public DateOnly? Fecha_Fin { get; set; }
    public List<int>? Departamentos { get; set; }
    public List<int>? Municipios { get; set; }
    public int? Estado { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}