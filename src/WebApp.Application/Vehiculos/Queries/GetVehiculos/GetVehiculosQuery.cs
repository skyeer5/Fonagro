using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Vehiculos.Queries.GetVehiculos;

public class GetVehiculosQuery
{
    public record GetVehiculosQueryRequest : IRequest<Result<GetVehiculosResponse>>
    {
        public GetVehiculosRequest? request { get; set; }
    }
}