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

    public NombramientoRepository(WebAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateNombramientoAsync(NombramientoCreateRequest request, int correlativo, CancellationToken cancellationToken)
    {
        var nombramiento = Nombramiento.Crear(request.UsuarioId, request.Proposito!, request.Fecha_Salida, request.Fecha_Regreso, request.Municipios!, correlativo);
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
}