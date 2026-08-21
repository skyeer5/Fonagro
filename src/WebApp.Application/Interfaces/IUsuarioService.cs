using WebApp.Application.Core;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivos;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;
using WebApp.Application.Usuarios.Queries.GetUsuariosSinComision;
using WebApp.Application.Usuarios.Queries.GetUsuariosSinNom;
using static WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle.GetUsuariosActivosDetalleQuery;

namespace WebApp.Application.Interfaces;

public interface IUsuarioService
{
    Task<bool> UsuarioExistsAsync(int usuarioId, CancellationToken cancellationToken);
    Task<bool> UsuariosExistsAsync(int usuarioId, List<int> usuariosIds);
    Task<List<GetUsuariosActivosResponse>> GetUsuariosActivosAsync();
    Task<Result<PagedList<GetUsuariosActivosDetalleResponse>>> GetUsuariosActivosDetalleAsync(GetUsuariosActivosDetalleQueryRequest request);
    Task<List<GetUsuariosSinComisionResponse>> GetUsuariosSinComisionAsync();
    Task<string?> GetNombreUsuarioAsync(int usuarioId); 
    Task<List<GetUsuariosSinNomResponse>> GetUsuariosSinNomAsync();
}