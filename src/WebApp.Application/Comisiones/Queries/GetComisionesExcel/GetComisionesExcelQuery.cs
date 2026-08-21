using MediatR;
using WebApp.Application.Core;

namespace WebApp.Application.Comisiones.Queries.GetComisionesExcel;

public class GetComisionesExcelQuery
{
    public record GetComisionesExcelQueryRequest(GetComisionesExcelRequest request) : IRequest<Result<byte[]>>;
}