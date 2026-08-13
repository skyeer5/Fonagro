using System.Diagnostics;
using Microsoft.Extensions.Logging;
using WebApp.Application.Interfaces;

namespace WebApp.Infrastructure.Services;

public class NombramientoPdfService : INombramientoPdfService
{
private static readonly SemaphoreSlim _concurrencyLimiter = new(initialCount: 2, maxCount: 2);
    private readonly ILogger<NombramientoPdfService> _logger;
    private readonly string _workingDir;
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(30);

    public NombramientoPdfService(ILogger<NombramientoPdfService> logger)
    {
        _logger = logger;
        _workingDir = "/tmp/nombramientos";
        Directory.CreateDirectory(_workingDir);
    }

    public async Task<byte[]> ConvertToPdfAsync(byte[] docxBytes, CancellationToken cancellationToken)
    {
        var jobId = Guid.NewGuid().ToString("N");
        var docxPath = Path.Combine(_workingDir, $"{jobId}.docx");
        var pdfPath = Path.Combine(_workingDir, $"{jobId}.pdf");
        var profileDir = Path.Combine(_workingDir, $"{jobId}_profile");

        await File.WriteAllBytesAsync(docxPath, docxBytes, cancellationToken);

        await _concurrencyLimiter.WaitAsync(cancellationToken);
        try
        {
            await RunSofficeAsync(docxPath, profileDir, cancellationToken);

            if (!File.Exists(pdfPath) || new FileInfo(pdfPath).Length == 0)
                throw new Exception(
                    "LibreOffice no generó un PDF válido.");

            return await File.ReadAllBytesAsync(pdfPath, cancellationToken);
        }
        finally
        {
            _concurrencyLimiter.Release();
            TryDelete(docxPath);
            TryDelete(pdfPath);
            TryDeleteDirectory(profileDir);
        }
    }

    private async Task RunSofficeAsync(string docxPath, string profileDir, CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "soffice",
            Arguments = $"--headless --norestore " +
                        $"-env:UserInstallation=file://{profileDir} " +
                        $"--convert-to pdf --outdir {_workingDir} {docxPath}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var process = Process.Start(startInfo)
            ?? throw new Exception("No se pudo iniciar soffice.");

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(_timeout);

        try
        {
            await process.WaitForExitAsync(timeoutCts.Token);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            process.Kill(entireProcessTree: true);
            throw new Exception("La conversión a PDF excedió el tiempo límite.");
        }

        if (process.ExitCode != 0)
        {
            var stderr = await process.StandardError.ReadToEndAsync();
            _logger.LogError("soffice falló (exit {Code}): {Error}", process.ExitCode, stderr);
            throw new Exception($"soffice terminó con código {process.ExitCode}.");
        }
    }

    private void TryDelete(string path)
    {
        try { if (File.Exists(path)) File.Delete(path); }
        catch (IOException ex) { _logger.LogWarning(ex, "No se pudo borrar {Path}", path); }
    }

    private void TryDeleteDirectory(string path)
    {
        try { if (Directory.Exists(path)) Directory.Delete(path, recursive: true); }
        catch (IOException ex) { _logger.LogWarning(ex, "No se pudo borrar {Path}", path); }
    }
}
