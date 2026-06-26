using WebApp.Domain;

namespace WebApp.Application.Comisiones.ComisionCreate;

public class ComisionCreateRequest
{
    public List<string>? Departamento { get; set; }
    public DateTime Fecha_Salida { get; set; }
    public DateTime Fecha_Regreso { get; set; }
    public int VehiculoId { get; set; }
    public List<Nombramiento> UsuariosNombrados { get; set; } = new();
}