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
    private readonly IUnidadService _unidadService;

    public NombramientoCreateCommandHandler(INombramientoRepository nombramientoRepository, IMunicipioService municipioService, INombramientoPolicy nombramientoPolicy, IUnidadService unidadService)
    {
        _nombramientoRepository = nombramientoRepository;
        _municipioService = municipioService;
        _nombramientoPolicy = nombramientoPolicy;
        _unidadService = unidadService;
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
        var unidad = _unidadService.GetUnidadIdByUsuarioIdAsync(request.request.UsuarioId!, cancellationToken);
        if(unidad is null)
        {
            return Result<int>.Failure("El usuario no tiene una unidad asignada.");
        }
        var correlativo = await _nombramientoRepository.ObtenerCorrelativoByUsuarioIdAsync(unidad.Result!.Value, cancellationToken);
        var nombramientoId = await _nombramientoRepository.CreateNombramientoAsync(request.request, correlativo, cancellationToken);
        return Result<int>.Success(nombramientoId);
    }
}