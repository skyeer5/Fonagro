using AutoMapper;
using WebApp.Application.Vehiculos.Queries.GetVehiculo;
using WebApp.Application.Vehiculos.Queries.GetVehiculos;
using WebApp.Domain;

namespace WebApp.Application.Core;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Vehiculo, GetVehiculoResponse>();
        CreateMap<Vehiculo, GetVehiculosResponse>();
    }
}