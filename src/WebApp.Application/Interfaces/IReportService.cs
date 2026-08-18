using WebApp.Application.Comisiones.Queries.PlanViajeExcel;

namespace WebApp.Application.Interfaces;

public interface IReportService
{
    byte[] GetExcelPlanViaje(PlanViajeResponse planViaje);
}