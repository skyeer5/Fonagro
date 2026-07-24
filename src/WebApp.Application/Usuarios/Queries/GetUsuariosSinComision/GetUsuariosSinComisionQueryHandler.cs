using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosSinComision;

public class GetUsuariosSinComisionQueryHandler : IRequestHandler<GetUsuariosSinComisionQuery.GetUsuariosSinComisionQueryRequest, Result<List<GetUsuariosSinComisionResponse>>>
{
    private readonly IUsuarioService _usuarioService;

    public GetUsuariosSinComisionQueryHandler(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<Result<List<GetUsuariosSinComisionResponse>>> Handle(GetUsuariosSinComisionQuery.GetUsuariosSinComisionQueryRequest request, CancellationToken cancellationToken)
    {
        var usuarios = await _usuarioService.GetUsuariosSinComisionAsync();
        if(usuarios is null)
        {
            return Result<List<GetUsuariosSinComisionResponse>>.Failure("Error al obtener los usuarios sin comisión.");
        }
        return Result<List<GetUsuariosSinComisionResponse>>.Success(usuarios);
    }
}