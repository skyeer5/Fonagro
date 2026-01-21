using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Vehiculos.Queries.GetVehiculo;

public class GetVehiculoQuery
{
    public record GetVehiculoQueryRequest : IRequest<Result<GetVehiculoResponse>>
    {
        public int Id { get; set; }
    };
}