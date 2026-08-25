namespace WebApp.Application.Nombramientos.Queries.NombramientoPdf;

public class NombramientoPdfResponse
{
    public byte[] Pdf { get; set; } = [];
    public string Correlativo { get; set; } = string.Empty;
}