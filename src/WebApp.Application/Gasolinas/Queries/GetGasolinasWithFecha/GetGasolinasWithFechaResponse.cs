namespace WebApp.Application.Gasolinas.Queries.GetGasolinasWithFecha;


public class GetGasolinasWithFechaResponse
{
    public string Descripcion { get; set; } = null!;
    public static GetGasolinasWithFechaResponse Crear(string? nombre, decimal precio, DateTime? fecha)
    {
        return new GetGasolinasWithFechaResponse
        {
            Descripcion = $"{nombre} - Precio: {precio} \n {fecha:dd/MM/yyyy HH:mm}"
        };
    }
}