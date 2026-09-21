namespace WebApp.Application.Comisiones.Command.ComisionAddDestinos;

public class ComisionAddDestinosItemRequest
{
    public int ComisionDestinoId { get; set; }
    public string? Descripcion { get; set;}
    public decimal Kilometro { get; set;}
}