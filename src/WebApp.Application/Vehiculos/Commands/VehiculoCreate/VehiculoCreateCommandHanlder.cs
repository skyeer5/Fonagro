using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Persistence;
using static WebApp.Application.Vehiculos.Commands.VehiculoCreate.VehiculoCreateCommand;

namespace WebApp.Application.Vehiculos.Commands.VehiculoCreate;

public sealed class VehiculoCreateCommandHandler : IRequestHandler<VehiculoCreateCommandRequest, Result<int>>
    {
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IGasolinaService _gasolinaService;
    private readonly IUsuarioService _usuarioService;

    public VehiculoCreateCommandHandler(IVehiculoRepository vehiculoRepository, IGasolinaService gasolinaService, IUsuarioService usuarioService)
    {
        _vehiculoRepository = vehiculoRepository;
        _gasolinaService = gasolinaService;
        _usuarioService = usuarioService;
    }

    public async Task<Result<int>> Handle(VehiculoCreateCommandRequest request, CancellationToken cancellationToken)
        {
            if(!await _gasolinaService.GasolinaExistsAsync(request.VehiculoCreateRequest.GasolinaId, cancellationToken))
            {
                return Result<int>.Failure("Tipo de gasolina no encontrado");
            }

            if(!await _usuarioService.UsuariosExistsAsync(request.VehiculoCreateRequest.Creado_Por))
            {
                return Result<int>.Failure("Usuario creador no encontrado");
            }
            
            var gasoNombre= await _gasolinaService.GetNombreByIdAsync(request.VehiculoCreateRequest.GasolinaId, cancellationToken);

            var vehiculo = Vehiculo.Crear(request.VehiculoCreateRequest.Placa!,
                                request.VehiculoCreateRequest.Marca!,
                                request.VehiculoCreateRequest.Modelo!,
                                request.VehiculoCreateRequest.Anio,
                                request.VehiculoCreateRequest.Tipo_Vehiculo!,
                                request.VehiculoCreateRequest.Color!,
                                request.VehiculoCreateRequest.Cilindraje!,
                                request.VehiculoCreateRequest.Kilometraje,
                                request.VehiculoCreateRequest.GasolinaId,
                                gasoNombre!,
                                request.VehiculoCreateRequest.Creado_Por);

            vehiculo.AgregarListaAccesorios(request.VehiculoCreateRequest.Accesorios);
            
            var partes = request.VehiculoCreateRequest.Partes
                .Select(p => Parte.CrearParteDeRequest(p.Id, p.Descripcion!))
                .ToList();

            vehiculo.AgregarListaPartes(partes);
            
            var resultado = await _vehiculoRepository.CreateVehiculoAsync(vehiculo, cancellationToken);

            return resultado.IsSuccess ? Result<int>.Success(vehiculo.VehiculoId) : Result<int>.Failure(resultado.Error!);
        }
    }