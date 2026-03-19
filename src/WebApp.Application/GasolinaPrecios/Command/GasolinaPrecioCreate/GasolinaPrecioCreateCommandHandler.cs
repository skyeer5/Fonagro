using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.GasolinaPrecios.Command.GasolinaPrecioCreate;

public class GasolinaPrecioCreateCommandHandler : IRequestHandler<GasolinaPrecioCreateCommand.GasolinaPrecioCreateCommandRequest, Result<int>>
{
    private readonly IGasolinaPrecioRepository _repository;

    public GasolinaPrecioCreateCommandHandler(IGasolinaPrecioRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<int>> Handle(GasolinaPrecioCreateCommand.GasolinaPrecioCreateCommandRequest request, CancellationToken cancellationToken)
    {
        return await _repository.CreateAsync(request.Request.GasolinaId, request.Request.Precio, cancellationToken);
    }
}