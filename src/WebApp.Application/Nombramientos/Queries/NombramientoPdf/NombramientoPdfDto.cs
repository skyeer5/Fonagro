namespace WebApp.Application.Nombramientos.Queries.NombramientoPdf;

public class NombramientoPdfDto
{
    public string NumeroNombramiento { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; } 

    public string NombreCompleto { get; set; } = string.Empty;

    public string Puesto { get; set; } = string.Empty;

    public string Proposito { get; set; } = string.Empty;

    public List<NombramientoPdfDestinosDto> Destinos { get; set; } = [];

    public DateTime FechaInicio { get; set; } 

    public DateTime FechaFin { get; set; } 

    public string EmitidoPor { get; set; } = string.Empty;
}