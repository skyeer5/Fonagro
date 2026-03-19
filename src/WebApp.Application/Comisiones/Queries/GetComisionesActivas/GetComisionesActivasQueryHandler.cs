using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Comisiones.Queries.GetComisionesActivas;

public class GetComisionActivaQueryHandler
            : IRequestHandler<GetComisionesActivasQuery.GetComisionActivaQueryRequest, Result<GetComisionActivaResponse>>
{
    private readonly IComisionService _comisionService;

    public GetComisionActivaQueryHandler(IComisionService comisionService)
    {
        _comisionService = comisionService;
    }

    public async Task<Result<GetComisionActivaResponse>> Handle(GetComisionesActivasQuery.GetComisionActivaQueryRequest request, CancellationToken cancellationToken)
    {
        var comisiones = await _comisionService.GetComisionActivaAsync();
        if (comisiones is null)
        {
             return Result<GetComisionActivaResponse>.Failure("No se encontró comisión activa para el usuario.");
        }

        return Result<GetComisionActivaResponse>.Success(comisiones);
    }
}