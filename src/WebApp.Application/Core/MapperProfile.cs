using AutoMapper;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;
using WebApp.Application.Vehiculos.Queries.GetVehiculo;
using WebApp.Application.Vehiculos.Queries.GetVehiculos;
using WebApp.Domain;
using WebApp.Persistence.Models;

namespace WebApp.Application.Core;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Vehiculo, GetVehiculoResponse>();
        CreateMap<Vehiculo, GetVehiculosResponse>();
        CreateMap<AppUser, GetUsuariosActivosDetalleResponse>();
    }
}