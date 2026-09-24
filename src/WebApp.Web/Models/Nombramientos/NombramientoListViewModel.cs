using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Nombramientos.Queries.GetNombramientos;

namespace WebApp.Web.Models.Nombramientos;

public class NombramientoListViewModel
{
    public int? Usuario { get; set; }
    public int? Correlativo { get; set; }
    public DateOnly? Fecha_Inicio { get; set; }
    public DateOnly? Fecha_Fin { get; set; }
    public int? Estado { get; set; }
    public int? Unidad { get; set; }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public List<GetNombramientosResponse> NombramientosList { get; set; } = [];
    public List<SelectListItem> UsuariosList { get; set; } = [];
    public List<SelectListItem> EstadoList { get; set; } = [];
    public List<SelectListItem> UnidadList { get; set; } = [];
}