using Microsoft.EntityFrameworkCore;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
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

    public void Add(Domain.Comisiones.Comision comision)
    {
        AgregarCreadoPor(comision);
        
        _context.Comisiones.Add(comision);
    } 
    public void AgregarCreadoPor(Domain.Comisiones.Comision comision)
    {
        var userId = _currentUser.userId;
        comision.AgregarCreadoPor(userId);
    }
    public void AgregarAprobadoPor(Domain.Comisiones.Comision comision)
    {
        var userId = _currentUser.userId;
        comision.AgregarAprobadoPor(userId);
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

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void RemoveRangeDestinos(List<ComisionDestino> destinos)
    {
        _context.ComisionDestinos.RemoveRange(destinos);
    }
}