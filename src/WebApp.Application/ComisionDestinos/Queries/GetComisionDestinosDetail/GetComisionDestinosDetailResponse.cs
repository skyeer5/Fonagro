namespace WebApp.Application.ComisionDestinos.Queries.GetComisionDestinosDetail;

public class GetComisionDestinosDetailResponse
{
    public string? Descripcion { get; set;}
    public decimal Kilometros { get; set;}
    public decimal Galones { get; set;}
    public decimal Total { get; set;}
    
    public static GetComisionDestinosDetailResponse Crear(string? descripcion, decimal kilometro, decimal galones, decimal precioGalon)
    {
        var total = galones * precioGalon;
        
        return new GetComisionDestinosDetailResponse
        {
            Descripcion = descripcion,
            Kilometros = kilometro,
            Galones = galones,
            Total = total
        };
    {
        
    }
    }
}   