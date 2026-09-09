namespace WebApp.Application.Nombramientos.Queries.GetNomDatosById;

public class GetNomDatosByIdResponse
{
    public string? Departamentos { get; set; }
    public string? Municipios { get; set; }
    public DateOnly Fecha_Salida { get; set; }
    public DateOnly Fecha_Regreso { get; set; }
}