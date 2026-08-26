using WebApp.Application.Comisiones.Queries.GetComisionesActivas;
using WebApp.Application.Comisiones.Queries.GetComisionesDetalle;
using WebApp.Application.Comisiones.Queries.GetComisionesExcel;
using WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;
using WebApp.Application.Comisiones.Queries.PlanViajePdf;
using WebApp.Application.Core;

namespace WebApp.Application.Interfaces;

public interface IComisionService
{
    Task<GetComisionActivaResponse?> GetComisionActivaAsync();
    Task<Result<PagedList<GetComisionesDetalleResponse>>> GetComisionesDetalleAsync(GetComisionesDetalleRequest request);
    Task<PagedList<GetComisionesPendApprovResponse>> GetComisionPendApprovAsync(GetComisionesPendApprovRequest request);
    Task<Domain.Comisiones.Comision?> GetComisionByIdAsync(int comisionId);
    Task<PlanViajeDto?> GetPlanViajeResponseAsync(int idUsuario, int idComision);
    Task<List<GetComisionesExcelDto>?> GetComisionesExcelDtos(GetComisionesExcelRequest request, CancellationToken cancellationToken);

}