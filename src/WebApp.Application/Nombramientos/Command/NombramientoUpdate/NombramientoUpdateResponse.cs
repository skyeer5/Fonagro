namespace WebApp.Application.Nombramientos.Command.NombramientoUpdate;

public class NombramientoUpdateResponse
{
    public int NombramientoId { get; set; }
    public string? Proposito { get; set; }
    public List<int>? Municipios { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }

}