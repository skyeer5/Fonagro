using MediatR;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Comisiones.Queries.PlanViajeExcel;

public class PlanViajeQuery
{
    public record PlanViajeQueryRequest(int idUsuario, int idComision) : IRequest<Byte[]>;
    internal class PlanViajeQueryHandler : IRequestHandler<PlanViajeQueryRequest, Byte[]>
    {
        private readonly IReportService _reportService;

        public PlanViajeQueryHandler(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<Byte[]> Handle(PlanViajeQueryRequest request, CancellationToken cancellationToken)
        {
            return await _reportService.GetExcelPlanViajeAsync(request.idUsuario, request.idComision);
        }
    }
}