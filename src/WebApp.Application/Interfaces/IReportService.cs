using WebApp.Application.Comisiones.Queries.PlanViajeExcel;

namespace WebApp.Application.Interfaces;

public interface IReportService
{
    Task<byte[]> GetExcelPlanViajeAsync( int idComision);
}