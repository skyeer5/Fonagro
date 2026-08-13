using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;

public class GetUsuariosActivosDetalleQueryHandler : IRequestHandler<GetUsuariosActivosDetalleQuery.GetUsuariosActivosDetalleQueryRequest, Result<PagedList<GetUsuariosActivosDetalleResponse>>>
{
    private readonly IAsignacionUsuarioService _asignacionUsuario;

    public GetUsuariosActivosDetalleQueryHandler(IAsignacionUsuarioService asignacionUsuario)
    {
        _asignacionUsuario = asignacionUsuario;
    }

    public async Task<Result<PagedList<GetUsuariosActivosDetalleResponse>>> Handle(GetUsuariosActivosDetalleQuery.GetUsuariosActivosDetalleQueryRequest request, CancellationToken cancellationToken)
    {
        return await _asignacionUsuario.GetUsuariosActivosDetalleAsync(request);
    }
}