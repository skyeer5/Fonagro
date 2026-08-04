using MediatR;
using Microsoft.IdentityModel.Tokens;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain.Viaticos;
using static WebApp.Application.Comision.ComisionCreate.ComisionCreateCommand;

namespace WebApp.Application.Comision.ComisionCreate;

public class ComisionCreateCommandHandler : IRequestHandler<ComisionCreateCommandRequest, Result<int>>
{
    private readonly IGasolinaPrecioService _gasolinaPrecioService;
    private readonly IVehiculoService _vehiculoService;
    private readonly IComisionRepository _comisionRepository;
    private readonly INombramientoPolicy _comisionUsuarioPolicy;
    private readonly IViaticosService _viaticosService;

    public ComisionCreateCommandHandler(IGasolinaPrecioService gasolinaPrecioService, IVehiculoService vehiculoService, IUsuarioService usuarioService, IComisionRepository comisionRepository, INombramientoPolicy comisionUsuarioPolicy, IViaticosService viaticosService)
    {
        _gasolinaPrecioService = gasolinaPrecioService;
        _vehiculoService = vehiculoService;
        _comisionRepository = comisionRepository;
        _comisionUsuarioPolicy = comisionUsuarioPolicy;
        _viaticosService = viaticosService;
    }

    public async Task<Result<int>> Handle(ComisionCreateCommandRequest request, CancellationToken cancellationToken)
    {
        var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(request.ComisionCreateRequest.VehiculoId, cancellationToken);
        if(vehiculo is null)
        {
            return Result<int>.Failure("Vehículo no encontrado");
        }
        vehiculo.ModificarEstadoEnComision();

        var gasolinaPrecio = await _gasolinaPrecioService.GetPrecioActualByVehiculoIdAsync(request.ComisionCreateRequest.VehiculoId, cancellationToken);
        if(gasolinaPrecio is null)
        {
            return Result<int>.Failure("No se pudo obtener el precio de gasolina para el vehículo especificado");
        }

        var usuarioAsignados = await _comisionUsuarioPolicy.UsuariosEstanAsignadosAsync(request.ComisionCreateRequest.UsuariosNom, cancellationToken);
        if(usuarioAsignados)
        {
            return Result<int>.Failure("El usuario(s) ya se encuentra en otra comisión");
        }

        var viaticosVigentes = await _viaticosService.GetViaticosVigentesAsync();
        if(viaticosVigentes.IsNullOrEmpty())
        {
            return Result<int>.Failure("No hay viáticos vigentes para asignar a la comisión");
        }
        var viaticos = new List<Viatico>();
        foreach(var viatico in viaticosVigentes)
        {
            viaticos.Add(new Viatico(viatico.Id, viatico.Nombre, viatico.Monto));
        }

        var comision = Domain.Comisiones.Comision.Crear(
            request.ComisionCreateRequest.Departamento!,
            request.ComisionCreateRequest.Fecha_Salida,
            request.ComisionCreateRequest.Fecha_Regreso,
            request.ComisionCreateRequest.VehiculoId,
            gasolinaPrecio.Value
            );
        comision.AgregarUsuarios(request.ComisionCreateRequest.UsuariosNom, viaticos, request.ComisionCreateRequest.Fecha_Salida, request.ComisionCreateRequest.Fecha_Regreso);

        

        var comisionAdded = await _comisionRepository.AddAsync(comision, cancellationToken);

        return comisionAdded.IsSuccess ? Result<int>.Success(comision.ComisionId) : Result<int>.Failure(comisionAdded.Error!);
    }
}