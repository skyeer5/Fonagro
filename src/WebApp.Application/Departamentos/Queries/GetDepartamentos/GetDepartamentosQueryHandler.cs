using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Persistence;
using static WebApp.Application.Departamentos.Queries.GetDepartamentos.GetDepartamentosQuery;

namespace WebApp.Application.Departamentos.Queries.GetDepartamentos;

public class GetDepartamentosQueryHandler : IRequestHandler<GetDepartamentosQueryRequest, Result<List<GetDepartamentosResponse>>>
{
    private readonly IDepartamentoService _departamentoService;

    public GetDepartamentosQueryHandler(IDepartamentoService departamentoService)
    {
        _departamentoService = departamentoService;
    }

    public async Task<Result<List<GetDepartamentosResponse>>> Handle(GetDepartamentosQueryRequest request, CancellationToken cancellationToken)
    {
        var departamentos = await _departamentoService.GetDepartamentosAsync();
        if(departamentos is null)
        {
            return Result<List<GetDepartamentosResponse>>.Failure("Error al obtener los departamentos");
        }
        return Result<List<GetDepartamentosResponse>>.Success(departamentos);
    }
}