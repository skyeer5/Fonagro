using WebApp.Application.Comisiones.Queries.PlanViajePdf;

namespace WebApp.Application.Interfaces;

public interface IPlanViajeReportService
{
    byte[] GetExcelPlanViaje(PlanViajeDto planViaje);
}