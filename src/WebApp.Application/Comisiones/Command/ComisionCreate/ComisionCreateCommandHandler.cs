using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain.Nombramientos;
using WebApp.Domain.Viaticos;
using static WebApp.Application.Comision.ComisionCreate.ComisionCreateCommand;

namespace WebApp.Application.Comision.ComisionCreate;

public class ComisionCreateCommandHandler : IRequestHandler<ComisionCreateCommandRequest, Result<int>>
{
    private readonly IGasolinaPrecioService _gasolinaPrecioService;
    private readonly IVehiculoService _vehiculoService;
    private readonly IComisionRepository _comisionRepository;
    private readonly INombramientoPolicy _NombramientoPolicy;
    private readonly IViaticosService _viaticosService;
    private readonly INombramientoService _nombramientoService;

    public ComisionCreateCommandHandler(IGasolinaPrecioService gasolinaPrecioService, IVehiculoService vehiculoService, IComisionRepository comisionRepository, INombramientoPolicy nombramientoPolicy, IViaticosService viaticosService, INombramientoService nombramientoService)
    {
        _gasolinaPrecioService = gasolinaPrecioService;
        _vehiculoService = vehiculoService;
        _comisionRepository = comisionRepository;
        _NombramientoPolicy = nombramientoPolicy;
        _viaticosService = viaticosService;
        _nombramientoService = nombramientoService;
    }

    public async Task<Result<int>> Handle(ComisionCreateCommandRequest request, CancellationToken cancellationToken)
    {
        var comision = Domain.Comisiones.Comision.Crear(request.ComisionCreateRequest.Hora_Salida, request.ComisionCreateRequest.Hora_Regreso);

        if(request.ComisionCreateRequest.VehiculoId is not null)
        {
            var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(request.ComisionCreateRequest.VehiculoId.Value, cancellationToken);
            if(vehiculo is null)
            {
                return Result<int>.Failure("Vehículo no encontrado");
            }
            vehiculo.ModificarEstadoEnComision();

            var gasolinaPrecio = await _gasolinaPrecioService.GetPrecioActualByVehiculoIdAsync(request.ComisionCreateRequest.VehiculoId.Value, cancellationToken);
            if(gasolinaPrecio is null)
            {
                return Result<int>.Failure("No se pudo obtener el precio de gasolina para el vehículo especificado");
            }
            comision.AgregarVehiculo(vehiculo.VehiculoId, gasolinaPrecio.Value);
        }

        var nombramientosAsignados = await _NombramientoPolicy.NombramientosEstanAsignadosAsync(request.ComisionCreateRequest.Nombramientos, cancellationToken);
            if(nombramientosAsignados)
            {
                return Result<int>.Failure("Uno o más de los nombramientos ya se encuentra en otra comisión.");
            }

        var viaticosVigentes = await _viaticosService.GetViaticosVigentesAsync();
            if(viaticosVigentes.Count == 0 || viaticosVigentes is null)
            {
                return Result<int>.Failure("No hay viáticos vigentes para asignar a la comisión");
            }
            var viaticos = new List<Viatico>();
            foreach(var viatico in viaticosVigentes)
            {
                viaticos.Add(new Viatico(viatico.Id, viatico.Nombre, viatico.Monto));
            }

        var nombramientos = await _nombramientoService.GetNombramientosByListIdsAsync(request.ComisionCreateRequest.Nombramientos, cancellationToken);
        if(nombramientos is null)
        {
            return Result<int>.Failure("Error al encontrar los nombramientos");
        }

        var nombramientosTienenMismosDatos = Nombramiento.TodosTienenMismosDatos(nombramientos);
        if(!nombramientosTienenMismosDatos)
        {
            return Result<int>.Failure("Los nombramientos no coinciden sus datos asignados para realizar la comisión.");
        }

        comision.AgregarUsuarios(nombramientos, viaticos);

        _comisionRepository.Add(comision);

        var comisionAdded = await _comisionRepository.SaveChangesAsync(cancellationToken);

        return comisionAdded > 0 ? Result<int>.Success(comision.ComisionId) : Result<int>.Failure("Error al crear la comisión");
    }
}