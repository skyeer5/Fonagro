using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using static WebApp.Application.Nombramientos.Queries.NombramientoPdf.NombramientoPdfQuery;

namespace WebApp.Application.Nombramientos.Queries.NombramientoPdf;

public class NombramientoPdfQueryHandler : IRequestHandler<NombramientoPdfQueryRequest, Result<byte[]>>
{
    private readonly INombramientoService _nombramientoService;
    private readonly IDocumentConverter _nombramientoPdfService;
    private readonly IWordTemplateFiller _wordTemplateFiller;

    public NombramientoPdfQueryHandler(INombramientoService nombramientoService, IDocumentConverter nombramientoPdfService, IWordTemplateFiller wordTemplateFiller)
    {
        _nombramientoService = nombramientoService;
        _nombramientoPdfService = nombramientoPdfService;
        _wordTemplateFiller = wordTemplateFiller;
    }

    public async Task<Result<byte[]>> Handle(NombramientoPdfQueryRequest request, CancellationToken cancellationToken)
    {
        var nombramiento = await _nombramientoService.GetNombramientoPdfDtoAsync(request.nombramientoId, cancellationToken);
        if(nombramiento is null)
        {
            return Result<byte[]>.Failure("Error al obtener datos del nombramiento.");
        }
        var word = _wordTemplateFiller.FillTemplate(@"wwwroot\Templates\Nombramiento template.docx",nombramiento);
        if(word is null)
        {
            return Result<byte[]>.Failure("Error al convertir nombramiento en pdf");
        }
        var pdf = await _nombramientoPdfService.ConvertToPdfAsync(word, ".docx" ,cancellationToken);
        if(pdf is null)
        {
            return Result<byte[]>.Failure("Error al convertir nombramiento en pdf");
        }
        return Result<byte[]>.Success(pdf);
    }
}