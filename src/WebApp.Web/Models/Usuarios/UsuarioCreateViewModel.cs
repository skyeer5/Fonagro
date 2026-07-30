using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;

namespace WebApp.Web.Models.Usuarios;

public class UsuarioCreateViewModel
{
    public string? Nombre_Completo { get; set; }
    public string? NIT { get; set; }
    public string? Puesto { get; set; }
    public int Unidad { get; set; }
    public int Tipo_Servicios { get; set; }
    public string? Numero_Contrato { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public List<SelectListItem> Unidades { get; set; } = [];
    public List<SelectListItem> Puestos { get; set; } = [];
    public List<SelectListItem> TipoServicios { get; set; } = [];
}