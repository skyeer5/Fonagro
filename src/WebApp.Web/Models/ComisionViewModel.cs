using WebApp.Application.Comisiones.Queries.GetComisionesActivas;

namespace WebApp.Web.Models;

public class ComisionViewModel
{
    public GetComisionActivaResponse? Comision { get; set; }
    public bool TieneComisionCreada { get; set; }
    public bool TieneDestinosDefinidos { get; set; }
    public bool TieneCombustiblesAprobados { get; set; }
    public bool TieneComisionLista { get; set; }
    public bool TieneComisionEnCurso { get; set; }
    
}