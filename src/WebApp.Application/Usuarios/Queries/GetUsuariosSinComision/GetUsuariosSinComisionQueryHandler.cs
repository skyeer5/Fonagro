using MediatR;
using Microsoft.IdentityModel.Tokens;
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
        var usuarios = await _usuarioService.getUsuariosSinComisionAsync();
        if(usuarios.IsNullOrEmpty())
        {
            return Result<List<GetUsuariosSinComisionResponse>>.Failure("No se encontraron usuarios sin comisión.");
        }
        return Result<List<GetUsuariosSinComisionResponse>>.Success(usuarios);
    }
}