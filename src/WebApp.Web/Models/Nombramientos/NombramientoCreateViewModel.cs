using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Web.Models.Nombramientos;

public class NombramientoCreateViewModel
{
    public string? Proposito { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public int UsuarioId { get; set; }
    public List<int> Municipios { get; set; } = [];
    public List<SelectListItem>? Departamentos { get; set; } = [];
    public List<SelectListItem>? Usuarios { get; set; } = [];
}