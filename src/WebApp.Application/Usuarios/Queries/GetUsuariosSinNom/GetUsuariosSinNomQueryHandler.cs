using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Usuarios.Queries.GetUsuariosSinNom.GetUsuariosSinNomQuery;

namespace WebApp.Application.Usuarios.Queries.GetUsuariosSinNom;

public class GetUsuariosSinNomQueryHandler : IRequestHandler<GetUsuariosSinNomQueryRequest, Result<List<GetUsuariosSinNomResponse>>>
{
    private readonly IUsuarioService _usuarioService;
    public GetUsuariosSinNomQueryHandler(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<Result<List<GetUsuariosSinNomResponse>>> Handle(GetUsuariosSinNomQueryRequest request, CancellationToken cancellationToken)
    {
        var usuarios = await _usuarioService.GetUsuariosSinNomAsync();
        if(usuarios is null)
        {
            Result<List<GetUsuariosSinNomResponse>>.Failure("Error al obtener los usuarios");
        }

        return Result<List<GetUsuariosSinNomResponse>>.Success(usuarios!);
    }
}