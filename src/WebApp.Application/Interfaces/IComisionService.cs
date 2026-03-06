using WebApp.Application.Comisiones.Queries.GetComisionesActivas;
using WebApp.Application.Comisiones.Queries.PlanViajeExcel;

namespace WebApp.Application.Interfaces;

public interface IComisionService
{
    Task<GetComisionActivaResponse?> GetComisionActivaAsync(int usuarioId);
    Task<Domain.Comision?> GetComisionByIdAsync(int comisionId);
    Task<PlanViajeResponse> GetPlanViajeResponseAsync(int idUsuario, int idComision);
}