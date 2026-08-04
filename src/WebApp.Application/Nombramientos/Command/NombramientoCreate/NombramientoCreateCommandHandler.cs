using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Nombramientos.Command.NombramientoCreate.NombramientoCreateCommand;

namespace WebApp.Application.Nombramientos.Command.NombramientoCreate;

public class NombramientoCreateCommandHandler : IRequestHandler<NombramientoCreateCommandRequest, Result<int>>
{
    private readonly INombramientoRepository _nombramientoRepository;
    private readonly IMunicipioService _municipioService;
    private readonly INombramientoPolicy _nombramientoPolicy;

    public NombramientoCreateCommandHandler(INombramientoRepository nombramientoRepository, IMunicipioService municipioService, INombramientoPolicy nombramientoPolicy)
    {
        _nombramientoRepository = nombramientoRepository;
        _municipioService = municipioService;
        _nombramientoPolicy = nombramientoPolicy;
    }

    public async Task<Result<int>> Handle(NombramientoCreateCommandRequest request, CancellationToken cancellationToken)
    {
        var municipiosExisten = _municipioService.MunicipiosExists(request.request.Municipios!);
        if(!municipiosExisten)
        {
            return Result<int>.Failure("Uno o más municipios no existen.");
        }
        var usuariosEstaNombrado = await _nombramientoPolicy.UsuarioEstaNombradoAsync(request.request.UsuarioId!, cancellationToken);
        if(usuariosEstaNombrado)
        {
            return Result<int>.Failure("El usuario ya tiene un nombramiento activo.");
        }
        var nombramientoId = await _nombramientoRepository.CreateNombramientoAsync(request.request, cancellationToken);
        return Result<int>.Success(nombramientoId);
    }
}