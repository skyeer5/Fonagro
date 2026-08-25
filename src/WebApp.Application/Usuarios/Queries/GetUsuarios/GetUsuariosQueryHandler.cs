using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Usuarios.Queries.GetUsuarios.GetUsuariosQuery;

namespace WebApp.Application.Usuarios.Queries.GetUsuarios;

public class GetUsuariosQueryHandler : IRequestHandler<GetUsuariosQueryRequest, Result<List<GetUsuariosResponse>>>
{
    private readonly IUsuarioService _usuarioService;

    public GetUsuariosQueryHandler(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<Result<List<GetUsuariosResponse>>> Handle(GetUsuariosQueryRequest request, CancellationToken cancellationToken)
    {
        var usuarios = await _usuarioService.GetUsuariosAsync(cancellationToken);
        if(usuarios is null)
        {
            return Result<List<GetUsuariosResponse>>.Failure("Error al obtener los usuarios");
        }
        return Result<List<GetUsuariosResponse>>.Success(usuarios);
    }
}