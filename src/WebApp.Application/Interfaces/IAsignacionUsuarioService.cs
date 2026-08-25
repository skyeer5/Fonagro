using WebApp.Application.Core;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;
using static WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle.GetUsuariosActivosDetalleQuery;

namespace WebApp.Application.Interfaces;
public interface IAsignacionUsuarioService
{
    Task<int?> GetAsignacionUsuarioIdByUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken);
    Task<Result<PagedList<GetUsuariosActivosDetalleResponse>>> GetUsuariosActivosDetalleAsync(GetUsuariosActivosDetalleRequest request);

}