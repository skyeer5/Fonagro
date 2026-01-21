using WebApp.Persistence;
using WebApp.Application.Vehiculos.Commands.VehiculoCreate;
using WebApp.Application.Tests.Common;
using WebApp.Domain;
using static WebApp.Application.Vehiculos.Commands.VehiculoCreate.VehiculoCreateCommand;
using FluentAssertions;
using WebApp.Persistence.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Application.Vehiculos.Commands.VehiculoCreate;

public class VehiculoCreateCommandHandlerTests
{
    private readonly WebAppDbContext _context;
    private readonly VehiculoCreateCommandHandler _handler;

    public VehiculoCreateCommandHandlerTests()
    {
        _context = DbContextMockFactory.Create();
        _handler = new VehiculoCreateCommandHandler(_context);
    }

    [Fact]
    public async Task Should_Create_Vehiculo_Failure()
    {

        var command = new VehiculoCreateCommandRequest(
            new VehiculoCreateRequest
            {
                Placa = "P-123ABC",
            }
        );
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        _context.Vehiculos.Should().HaveCount(0);
    }

    [Fact]
    public async Task Should_Create_Vehiculo_Failure_Max_Lenght_Placa()
    {
        _context.Gasolinas.Add(new Gasolina { GasolinaId = 1, Nombre = "Regular" });
        _context.Users.Add(new AppUser { Id = 1, UserName = "testuser" });
        await _context.SaveChangesAsync();
        var command = new VehiculoCreateCommandRequest(
            new VehiculoCreateRequest
            {
                Placa = "P-123ABCasdfasdfasdfsadfadsfadfasdfasdfadf",
                GasolinaId = 1,
                Creado_Por = 1
            }
        );
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        _context.Vehiculos.Should().HaveCount(0);
    }

}