using WebApp.Application.Comisiones.Command.ComisionAddDestinos;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;

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

    public async Task<Result<int>> AddAsync(Domain.Comision comision, CancellationToken cancellationToken)
    {
        var userId = _currentUser.userId;
        comision.AgregarCreadoPor(userId);
        
        await _context.Comisiones.AddAsync(comision, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0 ? Result<int>.Success(comision.ComisionId) : Result<int>.Failure("Error al agregar la comisión");
    } 
    public async Task<Result<int>> AddApprovalGasAsync(Comision comision, CancellationToken cancellationToken)
    {
        var userId = _currentUser.userId;
        comision.AgregarAprobadoPor(userId);

        _context.Entry(comision).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        var resultado = await _context.SaveChangesAsync(cancellationToken);

        return resultado > 0 ? Result<int>.Success(resultado) : Result<int>.Failure("Error al aprobar el presupuesto de gasolina para la comisión");
    }
    public async Task<Result<int>> UpdateComisionAsync(Comision comision, CancellationToken cancellationToken)
    {
        _context.Entry(comision).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        var resultado = await _context.SaveChangesAsync(cancellationToken);

        return resultado > 0 ? Result<int>.Success(resultado) : Result<int>.Failure("Error al modificar la comisión");
    }
    public async Task<Result<int>> CheckComisionStatusAsync(CancellationToken cancellationToken)
    {
        var comisiones = _context.Comisiones.Where(c => c.Estado == EstadosTipos.Programada || c.Estado == EstadosTipos.EnCurso).ToList();
        foreach (var comision in comisiones)
        {
            if (comision.Estado == EstadosTipos.EnCurso && comision.Fecha_Regreso <= DateTime.Now)
            {
                comision.Estado = EstadosTipos.Completada;
                _context.Entry(comision).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else if (comision.Estado == EstadosTipos.Programada && comision.Fecha_Salida <= DateTime.Now)
            {
                comision.Estado = EstadosTipos.EnCurso;
                _context.Entry(comision).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            
        }
        return await _context.SaveChangesAsync(cancellationToken) > 0 ? Result<int>.Success(1) : Result<int>.Failure("Error al actualizar el estado de las comisiones");
    }

    public async Task<Result<int>> AddDestinosAsync(Domain.Comision comision, List<ComisionAddDestinosItemRequest> items, CancellationToken cancellationToken)
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
}