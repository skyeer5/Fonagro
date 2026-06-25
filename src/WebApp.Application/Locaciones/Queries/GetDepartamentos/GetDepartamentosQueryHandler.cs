using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Locaciones.Queries.GetDepartamentos;

public class GetDepartamentosQueryHandler : IRequestHandler<GetDepartamentosQuery.GetDepartamentosQueryRequest, Result<GetDepartamentosResponse>>
{
    private readonly ILocacionesService _locacionesService;

    public GetDepartamentosQueryHandler(ILocacionesService locacionesService)
    {
        _locacionesService = locacionesService;
    }

    public async Task<Result<GetDepartamentosResponse>> Handle(GetDepartamentosQuery.GetDepartamentosQueryRequest request, CancellationToken cancellationToken)
    {
        var departamentos = await _locacionesService.GetDepartamentosAsync();
        if(departamentos is null)
        {
            return Result<GetDepartamentosResponse>.Failure("Error al obtener los departamentos.");
        }


        var response = new GetDepartamentosResponse
        {
            Departamentos = departamentos
        };
        return Result<GetDepartamentosResponse>.Success(response);
    }
}