using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Application.Vehiculos.Queries.GetVehiculosDetalle;
using WebApp.Domain;
using WebApp.Domain.Vehiculos;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class VehiculoService : IVehiculoService
{
    private readonly WebAppDbContext _context;
    private readonly IMapper _mapper;

    public VehiculoService(WebAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<Vehiculo>> GetVehiculosDisponiblesAsync(CancellationToken cancellationToken)
    {
        return _context.Vehiculos
            .Where(v => v.Estado == EstadosTipos.Disponible) 
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
        IQueryable<Vehiculo> query = _context.Vehiculos.AsNoTracking();

        var predicate = ExpressionBuilder.New<Vehiculo>();

        if(!string.IsNullOrEmpty(request.Marca))
        {       
            predicate = predicate.And(x=>
                            x.Marca!
                            .Contains(request.Marca)
                        );
        }
        if(!string.IsNullOrEmpty(request.Modelo))
        {
            predicate = predicate.And(x=>
                            x.Modelo!
                            .Contains(request.Modelo)
                        );
        }
        if(!string.IsNullOrEmpty(request.Placa))
        {
            predicate = predicate.And(x=>
                            x.Placa!
                            .Contains(request.Placa)
                        );
        }
        if(!string.IsNullOrEmpty(request.Estado))
        {
            predicate = predicate.And(x=>
                            x.Estado!
                            .Contains(request.Estado)
                        );
        }
        if(!string.IsNullOrEmpty(request.OrderBy))
        {
            Expression<Func<Vehiculo, object>> orderBySelector =
                        request.OrderBy.ToLower() switch
                        {
                            "marca" => v => v.Marca!,
                            "modelo" => v => v.Modelo!,
                            "placa" => v => v.Placa!,
                            "estado" => v => v.Estado!,
                            _ => v => v.VehiculoId
                        };
            bool orderBy = request.OrderAsc.HasValue
                            ? request.OrderAsc.Value
                            : true;
            query = orderBy ? query.OrderBy(orderBySelector) : query.OrderByDescending(orderBySelector);
        }
        query = query.Where(predicate);

        var vehiculoQuery = query.ProjectTo<GetVehiculosDetalleResponse>(_mapper.ConfigurationProvider).AsQueryable();
        var pagination = await PagedList<GetVehiculosDetalleResponse>.CreateAsync(
                                    vehiculoQuery,
                                    request.PageNumber,
                                    request.PageSize
                                    );
        return Result<PagedList<GetVehiculosDetalleResponse>>.Success(pagination);
    }
}