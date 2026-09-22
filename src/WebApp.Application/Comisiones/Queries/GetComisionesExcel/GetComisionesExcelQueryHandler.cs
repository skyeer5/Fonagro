using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Comisiones.Queries.GetComisionesExcel.GetComisionesExcelQuery;

namespace WebApp.Application.Comisiones.Queries.GetComisionesExcel;

public class GetComisionesExcelQueryHandler : IRequestHandler<GetComisionesExcelQueryRequest, Result<byte[]>>
{
    private readonly IComisionService _comisionService;
    private readonly IMunicipioService _municipioService;
    private readonly IDepartamentoService _departamentoService;
    private readonly IUsuarioService _usuarioService;
    private readonly IVehiculoService _vehiculoService;
    private readonly IUnidadService _unidadService;
    private readonly IComisionReportService _comisionReportService;

    public GetComisionesExcelQueryHandler(IComisionService comisionService, IMunicipioService municipioService, IDepartamentoService departamentoService, IUsuarioService usuarioService, IVehiculoService vehiculoService, IUnidadService unidadService, IComisionReportService comisionReportService)
    {
        _comisionService = comisionService;
        _municipioService = municipioService;
        _departamentoService = departamentoService;
        _usuarioService = usuarioService;
        _vehiculoService = vehiculoService;
        _unidadService = unidadService;
        _comisionReportService = comisionReportService;
    }

    public async Task<Result<byte[]>> Handle(GetComisionesExcelQueryRequest request, CancellationToken cancellationToken)
    {
        if(request.request.Municipios is not null && request.request.Municipios.Count != 0)
        {
            var municipiosExisten = await _municipioService.MunicipiosExistsAsync(request.request.Municipios, cancellationToken);
            if(!municipiosExisten) 
                return Result<byte[]>.Failure("No se encontraron los municipios."); 
        }
        else
        {
            if(request.request.Departamentos is not null && request.request.Departamentos.Count != 0)
            {
                var departamentoExisten = await _departamentoService.DepartamentosExistAsync(request.request.Departamentos, cancellationToken);
                if(!departamentoExisten) 
                    return Result<byte[]>.Failure("No se encontraron los departamentos."); 
            }
        }
        if(request.request.Vehiculo.HasValue)
        {
            var vehiculoExiste = await _vehiculoService.VehiculoExistsAsync(request.request.Vehiculo.Value, cancellationToken);
            if(!vehiculoExiste)
                return Result<byte[]>.Failure("No se encontró el vehiculo."); 
        }
        if(request.request.Unidades is not null && request.request.Unidades.Count != 0)
        {
            var unidadesExisten = await _unidadService.UnidadesExistAsync(request.request.Unidades, cancellationToken);
            if(!unidadesExisten)
                return Result<byte[]>.Failure("No se encontraron las unidades."); 
        }
        if(request.request.Usuario.HasValue)
        {
            var usuarioExiste = await _usuarioService.UsuarioExistsAsync(request.request.Usuario.Value, cancellationToken);
            if(!usuarioExiste)
                return Result<byte[]>.Failure("No se encontró el usuario."); 
        }
        
        var comisiones = await _comisionService.GetComisionesExcelDtos(request.request, cancellationToken);
        if(comisiones is null)
        {
            return Result<byte[]>.Failure("Error al buscar las comisiones."); 
        }

        var excel = _comisionReportService.GetExcelComisiones(comisiones, cancellationToken);
        if(excel is null)
        {
            return Result<byte[]>.Failure("Error al realizar el excel."); 
        }
        return Result<byte[]>.Success(excel);  
    }
}