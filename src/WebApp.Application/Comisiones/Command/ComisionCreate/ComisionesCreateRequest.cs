using WebApp.Domain;

namespace WebApp.Application.Comisiones.ComisionCreate;

public class ComisionCreateRequest
{
    public int? VehiculoId { get; set; }
    public List<int> Nombramientos { get; set; } = new();
}