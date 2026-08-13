using WebApp.Application.Nombramientos.Queries.NombramientoPdf;

namespace WebApp.Application.Interfaces;

public interface INombramientoPdfService
{
    Task<byte[]> ConvertToPdfAsync(byte[] nombramiento, CancellationToken cancellationToken);
}