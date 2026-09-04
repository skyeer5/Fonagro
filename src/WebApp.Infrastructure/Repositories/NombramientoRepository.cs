using System.Data;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Application.Nombramientos.Command.NombramientoCreate;
using WebApp.Domain.Nombramientos;
using WebApp.Domain.Unidades;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Repositories;

public class NombramientoRepository : INombramientoRepository
{
    private readonly WebAppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public NombramientoRepository(WebAppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<int> CreateNombramientoAsync(NombramientoCreateRequest request, int correlativo, CancellationToken cancellationToken)
    {
        var user = _currentUser.userId;
        var nombramiento = Nombramiento.Crear(request.UsuarioId, request.Proposito!, request.Fecha_Salida, request.Fecha_Regreso, request.Municipios!, correlativo, user);
        await _context.Nombramientos.AddAsync(nombramiento);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0 ? nombramiento.NombramientoId : result;
    }

    public async Task<int> ObtenerCorrelativoByUsuarioIdAsync(UnidadesEnum unidad, CancellationToken cancellationToken)
    {
        var nombreSecuencia = unidad switch
        {
            UnidadesEnum.GERENCIA => "SeqGerencia",
            UnidadesEnum.UA => "SeqUA",
            UnidadesEnum.UAJ => "SeqUAJ",
            UnidadesEnum.UTSE => "SeqUTSE",
            UnidadesEnum.UDAI => "SeqUDAI",
            _=> throw new ArgumentOutOfRangeException(nameof(unidad))
        };

        var connection = _context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();

        command.CommandText = $"SELECT NEXT VALUE FOR {nombreSecuencia}";

        var result = await command.ExecuteScalarAsync(cancellationToken);

        return Convert.ToInt32(result);
    }
    
    public async Task<int> AprobarNombramientoAsync(int nombramientoId, CancellationToken cancellationToken)
    {
        var usuarioAprobador = _currentUser.userId;
        var nombramiento =  await _context.Nombramientos.FindAsync(nombramientoId);
        if(nombramiento is null)
        {
            return 0;
        }
        nombramiento.AprobarNombramiento(usuarioAprobador);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result;
    }

    public async Task CompletarNombramientoStatusAsync(CancellationToken cancellationToken)
    {
        var nombramientos = await _context.Nombramientos.Where(x=> x.Estado == NombramientoEstados.Aprobado 
                                                                && x.Comision == null 
                                                                && x.Fecha_Regreso <= DateTime.Now)
                                                        .ToListAsync(cancellationToken);
        if(nombramientos is null || nombramientos.Count == 0)
            return;
        foreach(var nom in nombramientos)
        {
            nom.Estado = NombramientoEstados.Completada;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelarNombramientoStatusAsync(CancellationToken cancellationToken)
    {
        var nombramientos = await _context.Nombramientos.Where(x=> x.Estado == NombramientoEstados.Creado 
                                                                && x.Fecha_Salida< DateTime.Now)
                                                        .ToListAsync(cancellationToken);
        if(nombramientos is null || nombramientos.Count == 0)
            return;
        foreach(var nom in nombramientos)
        {
            nom.Estado = NombramientoEstados.Cancelada;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}