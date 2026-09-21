using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain.ComisionDestinos;
using static WebApp.Application.Comisiones.Command.ComisionAddDestinos.ComisionAddDestinosCommand;

namespace WebApp.Application.Comisiones.Command.ComisionAddDestinos;

public class ComisionAddDestinosCommandHandler : IRequestHandler<ComisionAddDestinosCommandRequest, Result<int>>
{
    private readonly IComisionService _comisionService;
    private readonly IComisionRepository _comisionRepository;
    private readonly ICurrentUser _currentUser;

    public ComisionAddDestinosCommandHandler(IComisionService comisionService, IComisionRepository comisionRepository, ICurrentUser currentUser)
    {
        _comisionService = comisionService;
        _comisionRepository = comisionRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(ComisionAddDestinosCommandRequest request, CancellationToken cancellationToken)
    {
        var payload = request.request;
        var comision = await _comisionService.GetComisionByIdAsync(payload.ComisionId);

        if (comision is null)
            return Result<int>.Failure("No se ha encontrado la comisión");

        if (comision.Vehiculo is null)
            return Result<int>.Failure("La comisión no tiene un vehículo asignado para calcular el consumo");

        var consumoKmPorGalon = comision.Vehiculo.ConsumoKmPorGalon;
        var itemsRequest = payload.ComisionAddDestinosItemRequests ?? [];

        comision.ComisionDestinos ??= [];
        var destinosExistentes = comision.ComisionDestinos.ToList();

        var idsExistentes = itemsRequest.Where(d => d.ComisionDestinoId > 0).Select(d => d.ComisionDestinoId).ToList();

        var aEliminar = destinosExistentes.Where(e => !idsExistentes.Contains(e.ComisionDestinoId)).ToList();

        if (aEliminar.Count > 0)
            _comisionRepository.RemoveRangeDestinos(aEliminar);

        foreach (var dest in itemsRequest)
        {
            if (dest.ComisionDestinoId > 0)
            {
                var existente = destinosExistentes.FirstOrDefault(e => e.ComisionDestinoId == dest.ComisionDestinoId);
                existente?.Modificar(dest.Descripcion ?? string.Empty, dest.Kilometro, consumoKmPorGalon);
            }
            else
            {
                var nuevoDestino = ComisionDestino.Crear(dest.Descripcion ?? string.Empty, dest.Kilometro, consumoKmPorGalon);
                comision.ComisionDestinos.Add(nuevoDestino);
            }
        }

        comision.DefinirPrespuestoCombustible();

        _comisionRepository.AgregarCreadoPor(comision);
        var guardadoExitoso = await _comisionRepository.SaveChangesAsync(cancellationToken);

        return guardadoExitoso > 0 ? Result<int>.Success(comision.ComisionId) : Result<int>.Failure("Error al guardar los destinos de la comisión");
    }
}