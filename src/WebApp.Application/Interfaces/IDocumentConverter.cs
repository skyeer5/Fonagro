using WebApp.Application.Nombramientos.Queries.NombramientoPdf;

namespace WebApp.Application.Interfaces;

public interface IDocumentConverter
{
    Task<byte[]> ConvertToPdfAsync(byte[] documentBytes, string sourceExtension, CancellationToken cancellationToken);
}