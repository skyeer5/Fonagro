using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Vehiculos.Queries.GetVehiculo;
using WebApp.Application.Vehiculos.Queries.GetVehiculos;
using WebApp.Application.Vehiculos.Queries.GetVehiculosDetalle;
using WebApp.Domain.Vehiculos;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class VehiculoService : IVehiculoService
{
    private readonly WebAppDbContext _context;

    public VehiculoService(WebAppDbContext context)
    {
        _context = context;
    }

    public Task<List<Vehiculo>> GetVehiculosDisponiblesAsync(CancellationToken cancellationToken)
    {
        return _context.Vehiculos
            .Where(v => v.Estado == VehiculoEstados.Disponible) 
            .ToListAsync(cancellationToken);
    }

    public Task<bool> VehiculoExistsAsync(int vehiculoId, CancellationToken cancellationToken)
    {
        return _context.Vehiculos.AnyAsync(x => x.VehiculoId == vehiculoId, cancellationToken);
    }

    public Task<Vehiculo?> GetVehiculoByIdAsync(int vehiculoId, CancellationToken cancellationToken)
    {
        return _context.Vehiculos.FirstOrDefaultAsync(x => x.VehiculoId == vehiculoId, cancellationToken);
    }

    public async Task<Result<PagedList<GetVehiculosDetalleResponse>>> GetVehiculosDetalleAsync(GetVehiculosDetalleRequest request)
    {
        var query = _context.Vehiculos.AsNoTracking().AsQueryable();

        if(!string.IsNullOrEmpty(request.Marca))
            query = query.Where(x=> x.Marca!.Contains(request.Marca));
        
        if(!string.IsNullOrEmpty(request.Modelo))
            query = query.Where(x=> x.Modelo!.Contains(request.Modelo));

         if(!string.IsNullOrEmpty(request.Placa))
            query = query.Where(x=>x.Placa!.Contains(request.Placa));

        bool orderBy = request.OrderAsc.HasValue
                        ? request.OrderAsc.Value
                        : true;
        
        Expression<Func<Vehiculo, object>> orderBySelector =
                    request.OrderBy?.ToLower() switch
                    {
                        "marca" => v => v.Marca!,
                        "modelo" => v => v.Modelo!,
                        "placa" => v => v.Placa!,
                        _ => v => v.VehiculoId
                    };
        
        query = orderBy ? query.OrderBy(orderBySelector) : query.OrderByDescending(orderBySelector);
        var vehiculoQuery = query.Select(x=> new GetVehiculosDetalleResponse{
            VehiculoId = x.VehiculoId,
            Placa = x.Placa,
            Marca = x.Marca,
            Modelo = x.Modelo,
            Estado = x.Estado.ToString()
        });

        var pagination = await PagedList<GetVehiculosDetalleResponse>.CreateAsync(
                                    vehiculoQuery,
                                    request.PageNumber,
                                    request.PageSize
                                    );
        return Result<PagedList<GetVehiculosDetalleResponse>>.Success(pagination);
    }

    public async Task<GetVehiculoResponse?> GetVehiculoResponseByIdAsync(int vehiculoId, CancellationToken cancellationToken)
    {
        var query = await _context.Vehiculos.FindAsync(vehiculoId, cancellationToken);
        if(query is null)
            return null;
        
        return new GetVehiculoResponse
        {
            Placa = query.Placa,
            Marca = query.Marca,
            Modelo = query.Marca,
            Tipo_Vehiculo = query.Tipo_Vehiculo.ToString(),
            Color = query.Color,
            Cilindraje = query.Cilindraje.ToString(),
            ConsumoPorGalon = query.ConsumoKmPorGalon,
        };
    }

    public async Task<List<GetVehiculosResponse>?> GetVehiculosAsync(CancellationToken cancellationToken)
    {
        return await _context.Vehiculos.AsNoTracking().Select(x=> new GetVehiculosResponse
        {
            VehiculoId = x.VehiculoId,
            Descripcion = $"{x.Placa} / {x.Marca} {x.Modelo}"
        }).ToListAsync(cancellationToken);
    }
}