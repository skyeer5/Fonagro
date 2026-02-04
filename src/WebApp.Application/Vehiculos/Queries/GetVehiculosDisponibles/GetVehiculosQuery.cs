using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Vehiculos.Queries.GetVehiculosDisponibles;

public class GetVehiculosDisponiblesQuery
{
    public record GetVehiculosDisponiblesQueryRequest : IRequest<Result<List<GetVehiculosDisponiblesResponse>>>;
}