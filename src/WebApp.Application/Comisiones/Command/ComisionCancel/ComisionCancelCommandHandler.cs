using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Comisiones.Command.ComisionCancel.ComisionCancelCommand;

namespace WebApp.Application.Comisiones.Command.ComisionCancel;
public class ComisionCancelCommandHandler : IRequestHandler<ComisionCancelCommandRequest, Result<int>>
{
    private readonly IComisionService _comisionService;
    private readonly IComisionRepository _comisionRepository;

    public ComisionCancelCommandHandler(IComisionService comisionService, IComisionRepository comisionRepository)
    {
        _comisionService = comisionService;
        _comisionRepository = comisionRepository;
    }

    public async Task<Result<int>> Handle(ComisionCancelCommandRequest request, CancellationToken cancellationToken)
    {
        return await _comisionRepository.CancelComisionAsync(request.ComisionCancelRequest.ComisionId, cancellationToken);
    }
}