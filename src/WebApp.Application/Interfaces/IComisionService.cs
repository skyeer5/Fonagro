using WebApp.Application.Comisiones.Queries.GetComisionesActivas;
using WebApp.Application.Comisiones.Queries.GetComisionesDetalle;
using WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;
using WebApp.Application.Comisiones.Queries.PlanViajeExcel;
using WebApp.Application.Core;

namespace WebApp.Application.Interfaces;

public interface IComisionService
{
    Task<GetComisionActivaResponse?> GetComisionActivaAsync();
    Task<Result<PagedList<GetComisionesDetalleResponse>>> GetComisionesDetalleAsync(GetComisionesDetalleRequest request);
    Task<List<GetComisionesPendApprovResponse>?> GetComisionPendApprovAsync();
    Task<Domain.Comision?> GetComisionByIdAsync(int comisionId);
    Task<Result<PlanViajeResponse>> GetPlanViajeResponseAsync(int idUsuario, int idComision);

}