using AutoMapper;
using WebApp.Application.Comisiones.Queries.GetComisionesDetalle;
using WebApp.Application.Usuarios.Queries.GetUsuariosActivosDetalle;
using WebApp.Application.Vehiculos.Queries.GetVehiculo;
using WebApp.Application.Vehiculos.Queries.GetVehiculosDetalle;
using WebApp.Domain;
using WebApp.Domain.Vehiculos;
using WebApp.Persistence.Models;


namespace WebApp.Application.Core;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Vehiculo, GetVehiculosDetalleResponse>();
        CreateMap<Vehiculo, GetVehiculoResponse>();
        CreateMap<AppUser, GetUsuariosActivosDetalleResponse>()
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src =>
                src.ComisionUsuarios!.Any(cu => cu.Comision!.Estado != EstadosTipos.Finalizado && cu.Comision.Estado != EstadosTipos.Cancelada)
                    ? UsuarioEstados.EnComision
                    : src.Estado
            ));
        CreateMap<Domain.Comisiones.Comision, GetComisionesDetalleResponse>()
            .ForMember(dest => dest.Descripcion_Vehiculo, opt => opt.MapFrom(src =>
            $"{src.Vehiculo!.Placa} - {src.Vehiculo.Modelo}"
            ));
    }
}