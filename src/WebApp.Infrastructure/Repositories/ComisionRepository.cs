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
                                            .AsSplitQuery()
                                            .ToList();
        if(comisiones.Count == 0)
        {
            return Result<int>.Success(0);
        }
        else
        {
            foreach (var comision in comisiones)
            {
                if (comision.Estado == ComisionEstados.EnCurso && comision.Nombramiento_Respon_Vehiculo!.Fecha_Regreso.ToDateTime(comision.Hora_Regreso) <= DateTime.Now)
                {
                    comision.CompletarComision();
                }
                else if (comision.Estado == ComisionEstados.Programada && comision.Nombramiento_Respon_Vehiculo!.Fecha_Salida.ToDateTime(comision.Hora_Salida) <= DateTime.Now)
                {
                    comision.Estado = ComisionEstados.EnCurso;
                }
                else if((comision.Estado == ComisionEstados.Creada || comision.Estado == ComisionEstados.DestinosDefinidos) && comision.Nombramiento_Respon_Vehiculo!.Fecha_Salida.ToDateTime(comision.Hora_Salida) <= DateTime.Now)
                {
                    comision.CancelarComision();
                    foreach (var cu in comision.Nombramientos!)
                    {
                        _context.ComisionViaticos.RemoveRange(cu.ComisionViaticosList!);
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
            if(comision.Vehiculo is null)
            {
                var destino = ComisionDestino.Crear(com.Descripcion!, com.Kilometro);
                destinos.Add(destino);
            }
            else
            {
                var destino = ComisionDestino.Crear(com.Descripcion!, com.Kilometro, comision.Vehiculo!.ConsumoKmPorGalon);
                destinos.Add(destino);
            }
        }
        
        comision.AgregarDestinos(destinos, userId);

         var resultado = await _context.SaveChangesAsync(cancellationToken);

         return resultado > 0 ? Result<int>.Success(resultado) : Result<int>.Failure("Error al agregar los destinos a la comisión");
    }
    public async Task<int> CancelComisionAsync(Comision comision, CancellationToken cancellationToken)
    {
        foreach (var cu in comision.Nombramientos!)
        {
            cu.ComisionViaticosList?.Clear();
            cu.Comision = null;
        }
        var resultado = await _context.SaveChangesAsync(cancellationToken);
        return resultado;
    }
}