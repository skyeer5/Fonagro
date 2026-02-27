namespace WebApp.Application.ComisionViaticos.Queries.GetComisionViatico;

public class GetComisionViaticoResponse
{
    public int viaticoId { get; set; }
    public string? Tipo_viatico { get; set; }
    public decimal Monto { get; set; }
    public DateOnly Fecha { get; set; }
}