namespace WebApp.Application.Nombramientos.Queries.GetNomDatosById;

public class GetNomDatosByIdResponse
{
    public string? Departamentos { get; set; }
    public string? Municipios { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
}