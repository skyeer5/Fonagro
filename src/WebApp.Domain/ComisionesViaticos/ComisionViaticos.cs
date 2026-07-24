namespace WebApp.Domain.ComisionesViaticos;
using WebApp.Domain.Viaticos;
using WebApp.Domain.Nombramientos;

public class ComisionViaticos
{
    public int ComisionViaticosId { get; set; }
    public int Cantidad { get; set; }
    public decimal Monto_Unitario_Usado { get; set; }
    public decimal Monto_Total { get; set; }
    public DateTime Fecha {get;set;} 
    public int NombramientoId { get; set; }
    public Nombramiento? Nombramiento { get; set; }
    public int ViaticoId { get; set; }
    public Viatico? Viatico { get; set; }

    public static ComisionViaticos Crear(int cantidad, decimal precio_usado, DateTime fecha, int viaticoId, int nombramientoId)
    {
        return new ComisionViaticos
        {
            Cantidad = cantidad,
            Monto_Unitario_Usado = precio_usado,
            Monto_Total = precio_usado * cantidad,
            Fecha = fecha,
            NombramientoId = nombramientoId,
            ViaticoId = viaticoId
        };
    }

}