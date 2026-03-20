using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Gasolinas.Queries.GetGasolinasWithFecha;

public class GetGasolinasWithFechaQuery
{
    public record GetGasolinasWithFechaQueryRequest : IRequest<Result<List<GetGasolinasWithFechaResponse>>>;
}