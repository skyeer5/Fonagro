using WebApp.Application.Comisiones.Queries.GetComisionesExcel;
using WebApp.Application.Comisiones.Queries.PlanViajePdf;

namespace WebApp.Application.Interfaces;

public interface IComisionReportService
{
    byte[] GetExcelComisiones(List<GetComisionesExcelDto> comisionesExcelDto, CancellationToken cancellationToken);
}