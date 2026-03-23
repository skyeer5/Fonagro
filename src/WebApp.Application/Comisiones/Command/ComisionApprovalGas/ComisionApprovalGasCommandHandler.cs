using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Comisiones.Command.ComisionApprovalGas.ComisionApprovalGasCommand;

namespace WebApp.Application.Comisiones.Command.ComisionApprovalGas;

public class ComisionApprovalGasCommandHandler : IRequestHandler<ComisionApprovalGasCommandRequest, Result<int>>
{
    private readonly IComisionService _comisionService;
    private readonly IComisionRepository _comisionRepository;

    public ComisionApprovalGasCommandHandler(IComisionService comisionService, IComisionRepository comisionRepository)
    {
        _comisionService = comisionService;
        _comisionRepository = comisionRepository;
    }

    public async Task<Result<int>> Handle(ComisionApprovalGasCommandRequest request, CancellationToken cancellationToken)
    {
        var comision = await _comisionService.GetComisionByIdAsync(request.ComisionApprovalGasRequest.ComisionId);
        if(comision is null)
        {
            return Result<int>.Failure("Comision no encontrada");
        }
        comision.AgregarPresupuestoGas(request.ComisionApprovalGasRequest.PrespuestoAprobado);
        return await _comisionRepository.AddApprovalGas(comision,cancellationToken);
    }
}