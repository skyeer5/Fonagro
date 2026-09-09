using WebApp.Application.ComisionDestinos.Queries.GetComisionDestinos;

namespace WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;

public class GetComisionesPendApprovResponse
{
    public int ComisionId { get; set; }
    public List<string> DepartamentosYMunicipios { get; set; } = [];
    public List<string> Destinos { get; set; } = [];
    public DateOnly Fecha_Salida { get; set; }
    public DateOnly Fecha_Regreso { get; set; }
    public decimal Kilometros { get; set; }
    public decimal Precio_Gasolina { get; set; }
    public decimal Prespuesto_Estimado { get; set; }
    public decimal GalonesEstimados { get; set; }
    public decimal ComsumoKmPorGalon { get; set; }
    public decimal? PresupuestoAprobado { get; set; }
}