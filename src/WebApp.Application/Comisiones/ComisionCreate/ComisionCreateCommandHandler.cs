using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Persistence;
using static WebApp.Application.Comision.ComisionCreate.ComisionCreateCommand;

namespace WebApp.Application.Comision.ComisionCreate;

public class ComisionCreateCommandHandler : IRequestHandler<ComisionCreateCommandRequest, Result<int>>
{
    private readonly IGasolinaPrecioService _gasolinaPrecioService;
    private readonly IVehiculoService _vehiculoService;
    private readonly IUsuarioService _usuarioService;
    private readonly IComisionRepository _comisionRepository;
    private readonly IComisionUsuarioPolicy _comisionUsuarioPolicy;

    public ComisionCreateCommandHandler(IGasolinaPrecioService gasolinaPrecioService, IVehiculoService vehiculoService, IUsuarioService usuarioService, IComisionRepository comisionRepository, IComisionUsuarioPolicy comisionUsuarioPolicy)
    {
        _gasolinaPrecioService = gasolinaPrecioService;
        _vehiculoService = vehiculoService;
        _usuarioService = usuarioService;
        _comisionRepository = comisionRepository;
        _comisionUsuarioPolicy = comisionUsuarioPolicy;
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

        var usuarioExists = await _usuarioService.UsuariosExistsAsync(request.ComisionCreateRequest.UsuarioId);
        if(!usuarioExists)
        {
            return Result<int>.Failure("Usuario(s) sin resultado");
        }

        var usuarioAsignados = await _comisionUsuarioPolicy.UsuariosEstanAsignadosAsync(request.ComisionCreateRequest.UsuariosNombrados, cancellationToken);
        if(usuarioAsignados)
        {
            return Result<int>.Failure("El usuario(s) ya se encuentra en otra comisión");
        }

        var comision = Domain.Comision.Crear(
            request.ComisionCreateRequest.Departamento!,
            request.ComisionCreateRequest.Fecha_Salida,
            request.ComisionCreateRequest.Fecha_Regreso,
            request.ComisionCreateRequest.VehiculoId,
            gasolinaPrecio.Value,
            request.ComisionCreateRequest.UsuarioId
            );
        comision.AgregarUsuarios(request.ComisionCreateRequest.UsuariosNombrados);
    
        var comisionAdded = await _comisionRepository.AddAsync(comision, cancellationToken);

        return comisionAdded.IsSuccess ? Result<int>.Success(comision.ComisionId) : Result<int>.Failure(comisionAdded.Error!);
    }
}