using WebApp.Application.Comisiones.Queries.PlanViajePdf;

namespace WebApp.Application.Interfaces;

public interface IReportService
{
    byte[] GetExcelPlanViaje(PlanViajeResponse planViaje);
}