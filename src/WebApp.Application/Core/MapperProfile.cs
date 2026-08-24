using AutoMapper;
using WebApp.Application.Comisiones.Queries.GetComisionesDetalle;

namespace WebApp.Application.Core;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Domain.Comisiones.Comision, GetComisionesDetalleResponse>()
            .ForMember(dest => dest.Descripcion_Vehiculo, opt => opt.MapFrom(src =>
            $"{src.Vehiculo!.Placa} - {src.Vehiculo.Modelo}"
            ))
            .ForMember(dest=> dest.Fecha_Salida, opt => opt.MapFrom(src => 
            src.Nombramientos!.First().Fecha_Salida
            ))
            .ForMember(dest=> dest.Fecha_Regreso, opt => opt.MapFrom(src => 
            src.Nombramientos!.First().Fecha_Regreso
            ))
            .ForMember(dest=> dest.Departamento, opt => opt.MapFrom(src =>
            string.Join(", ", src.Nombramientos!.First().NomMunicipios!.Select(nm=>nm.Municipio!.Departamento.Nombre).Distinct())
            ));
    }
}