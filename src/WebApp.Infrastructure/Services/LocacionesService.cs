using System.Text.Json;
using WebApp.Application.Interfaces;
using WebApp.Application.Locaciones.Queries;

namespace WebApp.Infrastructure.Services;

public class LocacionesService : ILocacionesService
{
    public async Task<List<Departamentos>?> GetDepartamentosAsync()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Config", "Departamentos y municipios.json");
        var json = await File.ReadAllTextAsync(path);
        var departamentos = JsonSerializer.Deserialize<List<Departamentos>>(json);
        return departamentos;
    }
}