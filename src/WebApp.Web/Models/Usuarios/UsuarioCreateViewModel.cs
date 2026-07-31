using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Domain.Usuarios;

namespace WebApp.Web.Models.Usuarios;

public class UsuarioCreateViewModel
{
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? NIT { get; set; }
    public int Puesto { get; set; }
    public TipoServicios Tipo_Servicios { get; set; }
    public string? Numero_Contrato { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public List<SelectListItem> Unidades { get; set; } = [];
    public List<SelectListItem> Puestos { get; set; } = [];
    public List<SelectListItem> TipoServicios { get; set; } = [];
}