using WebApp.Application.Core;
using WebApp.Application.Usuarios.Commands.UsuarioCreate;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivos;
using WebApp.Application.Usuarios.Queries.GetUsuariosSinComision;

namespace WebApp.Application.Interfaces;

public interface IUsuarioService
{
    Task<bool> UsuariosExistsAsync(int usuarioId);
    Task<bool> UsuariosExistsAsync(int usuarioId, List<int> usuariosIds);
    Task<List<GetUsuariosActivosResponse>> getUsuariosActivosAsync();
    Task<List<GetUsuariosSinComisionResponse>> getUsuariosSinComisionAsync();
    Task<string?> GetNombreUsuarioAsync(int usuarioId); 
    
}