namespace WebApp.Application.Comisiones.Command.ComisionAddDestinos;

public class ComisionAddDestinosRequest
{
    public int ComisionId { get; set; }
    public List<ComisionAddDestinosItemRequest>? ComisionAddDestinosItemRequests { get; set; }
}