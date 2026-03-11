using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Gasolinas.Queries.GetGasolinas;

public class GetGasolinasQuery
{
    public record GetGasolinasQueryRequest : IRequest<Result<List<GetGasolinasResponse>>>;
}