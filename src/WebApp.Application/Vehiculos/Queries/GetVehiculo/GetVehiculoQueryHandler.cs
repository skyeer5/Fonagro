using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using WebApp.Application.Core;
using WebApp.Persistence;
using Microsoft.EntityFrameworkCore;
using static WebApp.Application.Vehiculos.Queries.GetVehiculo.GetVehiculoQuery;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WebApp.Application.Vehiculos.Queries.GetVehiculo;

public class GetVehiculoQueryHandler : IRequestHandler<GetVehiculoQueryRequest, Result<GetVehiculoResponse>>
{
    private readonly WebAppDbContext _context;
    private readonly IMapper _mapper;

    public GetVehiculoQueryHandler(WebAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<GetVehiculoResponse>> Handle(GetVehiculoQueryRequest request, CancellationToken cancellationToken)
    {
        var vehiculo = await _context.Vehiculos.Where(x=>x.VehiculoId == request.Id)
                                                .ProjectTo<GetVehiculoResponse>(_mapper.ConfigurationProvider)
                                                .FirstOrDefaultAsync(cancellationToken);


        if (vehiculo is null)
        {
            return Result<GetVehiculoResponse>.Failure("Vehículo no encontrado");
        }

        return Result<GetVehiculoResponse>.Success(vehiculo);
    }
}