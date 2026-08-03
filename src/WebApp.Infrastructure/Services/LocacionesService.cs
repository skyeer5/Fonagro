using System.Text.Json;
using WebApp.Application.Interfaces;
using WebApp.Domain.Departamentos;

namespace WebApp.Infrastructure.Services;

public class LocacionesService : ILocacionesService
{
    public async Task<List<Departamento>?> GetDepartamentosDataSeedAsync()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Config", "Departamentos y municipios.json");
        var json = await File.ReadAllTextAsync(path);
        var departamentos = JsonSerializer.Deserialize<List<DepartamentoJson>>(json);
        var departamentosList = Departamento.JsonToDepartamento(departamentos!);
        return departamentosList;
    }
}