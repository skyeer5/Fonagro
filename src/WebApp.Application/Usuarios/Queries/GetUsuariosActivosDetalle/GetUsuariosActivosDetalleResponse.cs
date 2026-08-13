using WebApp.Domain.Usuarios;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;

public class GetUsuariosActivosDetalleResponse
{
    public int Id { get; set; }
    public string? Nombre_Completo { get; set; }
    public string? NIT { get; set; }
    public string? Puesto { get; set; }
    public string? Unidad { get; set; }
    public UsuarioEstados Estado { get; set; }

}