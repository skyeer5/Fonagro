namespace WebApp.Application.Nombramientos.Command.NombramientoCreate;

public class NombramientoCreateRequest
{
    public string? Proposito { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public int UsuarioId { get; set; }
    public List<int>? Municipios { get; set; }
}