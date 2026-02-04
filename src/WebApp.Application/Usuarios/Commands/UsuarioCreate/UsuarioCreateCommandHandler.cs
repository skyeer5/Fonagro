using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;
using WebApp.Persistence.Models;

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