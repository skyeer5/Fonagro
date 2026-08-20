using MediatR;
using Microsoft.IdentityModel.Tokens;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosActivos;

public class GetUsuariosActivosQueryHandler : IRequestHandler<GetUsuariosActivosQuery.GetUsuariosActivosQueryRequest, Result<List<GetUsuariosActivosResponse>>>
{
    private readonly IUsuarioService _usuarioService;

    public GetUsuariosActivosQueryHandler(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<Result<List<GetUsuariosActivosResponse>>> Handle(GetUsuariosActivosQuery.GetUsuariosActivosQueryRequest request, CancellationToken cancellationToken)
    {
        var usuarios = await _usuarioService.GetUsuariosActivosAsync();
        if(usuarios.Count == 0 || usuarios is null)
        {
            return Result<List<GetUsuariosActivosResponse>>.Failure("No se encontraron usuarios activos.");
        }
        return Result<List<GetUsuariosActivosResponse>>.Success(usuarios);
    }
}