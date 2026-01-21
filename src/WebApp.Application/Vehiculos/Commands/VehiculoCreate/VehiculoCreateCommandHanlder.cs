using MediatR;
using WebApp.Application.Core;
using WebApp.Domain;
using WebApp.Persistence;
using static WebApp.Application.Vehiculos.Commands.VehiculoCreate.VehiculoCreateCommand;

namespace WebApp.Application.Vehiculos.Commands.VehiculoCreate;

public sealed class VehiculoCreateCommandHandler : IRequestHandler<VehiculoCreateCommandRequest, Result<int>>
    {
        private readonly WebAppDbContext _context;

        public VehiculoCreateCommandHandler(WebAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(VehiculoCreateCommandRequest request, CancellationToken cancellationToken)
        {
            var vehiculo = new Vehiculo
            {
                Placa = request.VehiculoCreateRequest.Placa,
                Marca = request.VehiculoCreateRequest.Marca,
                Modelo = request.VehiculoCreateRequest.Modelo,
                Anio = request.VehiculoCreateRequest.Anio,
                Tipo_Vehiculo = request.VehiculoCreateRequest.Tipo_Vehiculo,
                Color = request.VehiculoCreateRequest.Color,
                Capacidad_Pasajeros = request.VehiculoCreateRequest.Capacidad_Pasajeros,
                Tipo_Motor = request.VehiculoCreateRequest.Tipo_Motor,
                Kilometraje = request.VehiculoCreateRequest.Kilometraje,
                Estado = EstadosTipos.Disponible,
                Fecha_Creacion = DateTime.Now
            };

            if(request.VehiculoCreateRequest.GasolinaId is not null)
            {
                var gasolina = await _context.Gasolinas.FindAsync(request.VehiculoCreateRequest.GasolinaId , cancellationToken);
                if (gasolina is null)
                {
                    return Result<int>.Failure("Gasolina no encontrada");
                }
                vehiculo.GasolinaId = gasolina.GasolinaId;
            }
            if(request.VehiculoCreateRequest.Creado_Por is 0)
            {
                var usuario = await _context.Users.FindAsync(request.VehiculoCreateRequest.Creado_Por, cancellationToken);
                if (usuario is null)
                {
                    return Result<int>.Failure("Usuario no encontrado");
                }
                vehiculo.Creado_Por = usuario.Id;
            }
            await _context.Vehiculos.AddAsync(vehiculo, cancellationToken);
            var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;
            return resultado ? Result<int>.Success(0) : Result<int>.Failure("Error al crear el vehículo");
        }
    }