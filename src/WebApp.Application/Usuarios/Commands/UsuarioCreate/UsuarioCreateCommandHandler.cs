using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Usuarios.Commands.UsuarioCreate;

public class UsuarioCreateCommandHandler : IRequestHandler<UsuarioCreateCommand.UsuarioCreateCommandRequest, Result<int>>
{
    private readonly IUsuarioRepository _usuarioRepository;
    public UsuarioCreateCommandHandler( IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }
    public async Task<Result<int>> Handle(UsuarioCreateCommand.UsuarioCreateCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await _usuarioRepository.CreateUsuarioAsync(request.UsuarioCreateRequest, cancellationToken);
        return result;
    }
}