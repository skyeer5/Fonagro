namespace WebApp.Application.Gasolinas.Queries.GetGasolinasWithPrecio;


public class GetGasolinasWithPrecioResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public decimal? Precio { get; set; }
}