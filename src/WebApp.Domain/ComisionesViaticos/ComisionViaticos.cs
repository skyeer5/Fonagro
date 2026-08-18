namespace WebApp.Domain.ComisionesViaticos;
using WebApp.Domain.Viaticos;
using WebApp.Domain.Nombramientos;

public class ComisionViaticos
{
    public int ComisionViaticosId { get; set; }
    public int Cantidad { get; set; }
    public DateTime Fecha {get;set;} 
    public int NombramientoId { get; set; }
    public Nombramiento? Nombramiento { get; set; }
    public int ViaticoId { get; set; }
    public Viatico? Viatico { get; set; }

    public static ComisionViaticos Crear(int cantidad, DateTime fecha, int viaticoId)
    {
        return new ComisionViaticos
        {
            Cantidad = cantidad,
            Fecha = fecha,
            ViaticoId = viaticoId
        };
    }

}