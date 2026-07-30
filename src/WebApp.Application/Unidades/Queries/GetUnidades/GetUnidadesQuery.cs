using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Unidades.Queries.GetUnidades;

public class GetUnidadesQuery
{
    public record GetUnidadesQueryRequest : IRequest<Result<List<GetUnidadesResponse>>>;
}