using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Gasolinas.Queries.GetGasolinasWithPrecio;

public class GetGasolinasWithPrecioQuery
{
    public record GetGasolinasWithPrecioQueryRequest : IRequest<Result<List<GetGasolinasWithPrecioResponse>>>;
}