using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Comisiones.Command.ComisionAddDescripcion;

public class ComisionAddDescripcionCommandHandler : IRequestHandler<ComisionAddDescripcionQuery.ComisionAddDescripcionCommandRequest, Result<int>>
{
    private readonly IComisionUsuarioRepository _comisionRepository;
    private readonly INombramientoService _comisionService;

    public ComisionAddDescripcionCommandHandler(IComisionUsuarioRepository comisionRepository, INombramientoService comisionService)
    {
        _comisionRepository = comisionRepository;
        _comisionService = comisionService;
    }

    public async Task<Result<int>> Handle(ComisionAddDescripcionQuery.ComisionAddDescripcionCommandRequest request, CancellationToken cancellationToken)
    {
        var comision = await _comisionService.GetNMByIdComisionAndUsuarioIdAsync(request.request.IdComision, cancellationToken);
        if (comision is null) return Result<int>.Failure("Comisión no encontrada");

        comision.Descripcion = request.request.Descripcion;
        await _comisionRepository.UpdateAsync(comision, cancellationToken);

        return Result<int>.Success(comision.ComisionId);
    }
}