using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Usuarios.Commands.UsuarioCreate;

public class UsuarioCreateCommandHandler : IRequestHandler<UsuarioCreateCommand.UsuarioCreateCommandRequest, Result<int>>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPuestoService _puestoService;

    public UsuarioCreateCommandHandler(IUsuarioRepository usuarioRepository, IPuestoService puestoService)
    {
        _usuarioRepository = usuarioRepository;
        _puestoService = puestoService;
    }

    public async Task<Result<int>> Handle(UsuarioCreateCommand.UsuarioCreateCommandRequest request, CancellationToken cancellationToken)
    {
        if(!await _puestoService.ExistsAsync(request.UsuarioCreateRequest.Puesto))
        {
            return Result<int>.Failure("No se encontró el puesto");
        }
        return await _usuarioRepository.CreateUsuarioAsync(request.UsuarioCreateRequest, cancellationToken);
    }
}