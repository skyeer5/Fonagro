using MediatR;
using Microsoft.IdentityModel.Tokens;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;

public class GetUsuariosActivosDetalleQueryHandler : IRequestHandler<GetUsuariosActivosDetalleQuery.GetUsuariosActivosDetalleQueryRequest, Result<PagedList<GetUsuariosActivosDetalleResponse>>>
{
    private readonly IUsuarioService _usuarioService;

    public GetUsuariosActivosDetalleQueryHandler(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<Result<PagedList<GetUsuariosActivosDetalleResponse>>> Handle(GetUsuariosActivosDetalleQuery.GetUsuariosActivosDetalleQueryRequest request, CancellationToken cancellationToken)
    {
        return await _usuarioService.GetUsuariosActivosDetalleAsync(request);
    }
}