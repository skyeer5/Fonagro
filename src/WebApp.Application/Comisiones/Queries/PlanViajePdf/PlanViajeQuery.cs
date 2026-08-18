using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Comisiones.Queries.PlanViajePdf;

public class PlanViajeQuery
{
    public record PlanViajeQueryRequest(int idComision) : IRequest<Result<Byte[]>>;
    internal class PlanViajeQueryHandler : IRequestHandler<PlanViajeQueryRequest, Result<Byte[]>>
    {
        private readonly IReportService _reportService;
        private readonly IDocumentConverter _documentConverter;
        private readonly IComisionService _comisionService;
        private readonly ICurrentUser _currentUser;

        public PlanViajeQueryHandler(IReportService reportService, IDocumentConverter documentConverter, IComisionService comisionService, ICurrentUser currentUser)
        {
            _reportService = reportService;
            _documentConverter = documentConverter;
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

            var pdf = await _documentConverter.ConvertToPdfAsync(excel, ".xlsx" , cancellationToken);
            if(pdf is null)
            {
                return Result<Byte[]>.Failure("Error al convertir a pdf.");
            }
            return Result<byte[]>.Success(pdf);
        }
    }
}