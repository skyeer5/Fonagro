using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain.Comisiones;
using static WebApp.Application.Comisiones.Command.ComisionCancel.ComisionCancelCommand;

namespace WebApp.Application.Comisiones.Command.ComisionCancel;
public class ComisionCancelCommandHandler : IRequestHandler<ComisionCancelCommandRequest, Result<int>>
{
    private readonly IComisionRepository _comisionRepository;
    private readonly IComisionService _comisionService;

    public ComisionCancelCommandHandler(IComisionRepository comisionRepository, IComisionService comisionService)
    {
        _comisionRepository = comisionRepository;
        _comisionService = comisionService;
    }

    public async Task<Result<int>> Handle(ComisionCancelCommandRequest request, CancellationToken cancellationToken)
    {
        var comision = await _comisionService.GetComisionToCancelAsync(request.ComisionCancelRequest.ComisionId, cancellationToken);

        if(comision is null)       
            return Result<int>.Failure("Comision no encontrada");

        if(comision.Estado == ComisionEstados.Cancelada)
            return Result<int>.Failure("La comisión ya se encuentra cancelada");

        if(comision.Estado == ComisionEstados.Completada)
            return Result<int>.Failure("La comisión ya se encuentra finalizada, no se puede cancelar");
            
        comision.CancelarComision();
        var result = await _comisionRepository.SaveChangesAsync(cancellationToken);
        return result > 0 ?  Result<int>.Success(result) : Result<int>.Failure("Error al cancelar la comisión");
    }
}