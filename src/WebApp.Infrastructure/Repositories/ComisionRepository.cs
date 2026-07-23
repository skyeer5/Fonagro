using Microsoft.EntityFrameworkCore;
using WebApp.Application.Comisiones.Command.ComisionAddDestinos;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain.Nombramientos;
using WebApp.Persistence;
using WebApp.Domain.Comisiones;
using WebApp.Domain.ComisionDestinos;


namespace WebApp.Infrastructure.Repositories;

public class ComisionRepository : IComisionRepository
{
    private readonly WebAppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public ComisionRepository(WebAppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> AddAsync(Domain.Comisiones.Comision comision, CancellationToken cancellationToken)
    {
        var userId = _currentUser.userId;
        comision.AgregarCreadoPor(userId);
        
        await _context.Comisiones.AddAsync(comision, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0 ? Result<int>.Success(comision.ComisionId) : Result<int>.Failure("Error al agregar la comisión");
    } 
    public async Task<Result<int>> AddApprovalGasAsync(Domain.Comisiones.Comision comision, CancellationToken cancellationToken)
    {
        var userId = _currentUser.userId;
        comision.AgregarAprobadoPor(userId);

        _context.Entry(comision).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        var resultado = await _context.SaveChangesAsync(cancellationToken);

        return resultado > 0 ? Result<int>.Success(resultado) : Result<int>.Failure("Error al aprobar el presupuesto de gasolina para la comisión");
    }
    public async Task<Result<int>> UpdateComisionAsync(Domain.Comisiones.Comision comision, CancellationToken cancellationToken)
    {
        _context.Entry(comision).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        var resultado = await _context.SaveChangesAsync(cancellationToken);

        return resultado > 0 ? Result<int>.Success(resultado) : Result<int>.Failure("Error al modificar la comisión");
    }
    public async Task<Result<int>> CheckComisionStatusAsync(CancellationToken cancellationToken)
    {
        var comisiones = _context.Comisiones.Where(c => c.Estado != ComisionEstados.Completada && c.Estado != ComisionEstados.Cancelada)
                                            .Include(x=>x.Vehiculo)
                                            .Include(x=>x.Nombramientos!)
                                                .ThenInclude(cu=>cu.ComisionViaticosList)
                                            .ToList();
        if(comisiones.Count == 0)
        {
            return Result<int>.Success(0);
        }
        else
        {
            foreach (var comision in comisiones)
            {
                if (comision.Estado == ComisionEstados.EnCurso && comision.Fecha_Regreso <= DateTime.Now)
                {
                    comision.CompletarComision();
                }
                else if (comision.Estado == ComisionEstados.Programada && comision.Fecha_Salida <= DateTime.Now)
                {
                    comision.Estado = ComisionEstados.EnCurso;
                }
                else if((comision.Estado == ComisionEstados.Creada || comision.Estado == ComisionEstados.DestinosDefinidos) && comision.Fecha_Salida <= DateTime.Now)
                {
                    comision.CancelarComision();
                    if(comision.ComisionDestinos is not null)
                    {
                        _context.ComisionDestinos.RemoveRange(comision.ComisionDestinos);
                    }
                    foreach (var cu in comision.Nombramientos!)
                    {
                        _context.ComisionViaticos.RemoveRange(cu.ComisionViaticosList!);
                        cu.Estado = NombramientoTipos.Cancelada;
                    }
                }
                
            }
            return await _context.SaveChangesAsync(cancellationToken) > 0 ? Result<int>.Success(1) : Result<int>.Failure("Error al actualizar el estado de las comisiones");
        }
    }
    public async Task<Result<int>> AddDestinosAsync(Domain.Comisiones.Comision comision, List<ComisionAddDestinosItemRequest> items, CancellationToken cancellationToken)
    {
        var userId = _currentUser.userId;
        var destinos = new List<ComisionDestino>();
        foreach(var com in items)
        {
            var destino = ComisionDestino.Crear(com.Descripcion!, com.Kilometro, comision.Vehiculo!.ConsumoKmPorGalon);
            destinos.Add(destino);
        }
        
        comision.AgregarDestinos(destinos, userId);

         var resultado = await _context.SaveChangesAsync(cancellationToken);

         return resultado > 0 ? Result<int>.Success(resultado) : Result<int>.Failure("Error al agregar los destinos a la comisión");
    }
    public async Task<Result<int>> CancelComisionAsync(int comisionId, CancellationToken cancellationToken)
    {
        var comision = await _context.Comisiones
                                .Where(x=>x.ComisionId == comisionId)
                                .Include(x=>x.Vehiculo)
                                .Include(x=>x.ComisionDestinos)
                                .Include(x=>x.Nombramientos!)
                                    .ThenInclude(cu=>cu.ComisionViaticosList)
                                .FirstOrDefaultAsync(cancellationToken);
        if(comision is null)       
        {
            return Result<int>.Failure("Comision no encontrada");
        }
        if(comision.Estado == ComisionEstados.Cancelada)
        {
            return Result<int>.Failure("La comisión ya se encuentra cancelada");
        }
        if(comision.Estado == ComisionEstados.Completada)
        {
            return Result<int>.Failure("La comisión ya se encuentra finalizada, no se puede cancelar");
        }
        comision.CancelarComision();
        if(comision.ComisionDestinos is not null)
        {
            _context.ComisionDestinos.RemoveRange(comision.ComisionDestinos);
        }
        foreach (var cu in comision.Nombramientos!)
        {
            _context.ComisionViaticos.RemoveRange(cu.ComisionViaticosList!);
            cu.Estado = NombramientoTipos.Cancelada;
        }
        var resultado = await _context.SaveChangesAsync(cancellationToken);
        return resultado > 0 ? Result<int>.Success(resultado) : Result<int>.Failure("Error al cancelar la comisión");
    }
}