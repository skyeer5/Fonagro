namespace WebApp.Application.Comisiones.Command.ComisionAddDescripcion;

public class ComisionAddDescripcionRequest
{
    public int IdComision { get; set; }
    public int IdUsuario { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}