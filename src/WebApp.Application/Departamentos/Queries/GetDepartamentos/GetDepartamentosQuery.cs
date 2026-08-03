using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Departamentos.Queries.GetDepartamentos;

public class GetDepartamentosQuery
{
    public record GetDepartamentosQueryRequest : IRequest<Result<List<GetDepartamentosResponse>>>;
}