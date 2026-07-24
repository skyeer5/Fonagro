using WebApp.Application.Core;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;

public class GetUsuariosActivosDetalleRequest : PagingParameters
{
    public string? Nombre { get; set; }
    public string? Estado { get; set; }
}