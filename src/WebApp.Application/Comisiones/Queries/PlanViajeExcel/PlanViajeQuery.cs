using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Comisiones.Queries.PlanViajeExcel;

public class PlanViajeQuery
{
    public record PlanViajeQueryRequest(int idComision) : IRequest<Result<Byte[]>>;
    internal class PlanViajeQueryHandler : IRequestHandler<PlanViajeQueryRequest, Result<Byte[]>>
    {
        private readonly IReportService _reportService;
        private readonly IComisionService _comisionService;
        private readonly ICurrentUser _currentUser;

        public PlanViajeQueryHandler(IReportService reportService, IComisionService comisionService, ICurrentUser currentUser)
        {
            _reportService = reportService;
            _comisionService = comisionService;
            _currentUser = currentUser;
        }

        public async Task<Result<Byte[]>> Handle(PlanViajeQueryRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.userId;
            var comision = await _comisionService.GetPlanViajeResponseAsync(userId, request.idComision);
            if(comision is null)
            {
                return Result<Byte[]>.Failure("Error al obtener el plan de viaje.");
            }
            var excel = _reportService.GetExcelPlanViaje(comision);
            if(excel is null)
            {
                return Result<Byte[]>.Failure("Error al convertir a excel.");
            }
            return Result<byte[]>.Success(excel);
        }
    }
}