using Microsoft.EntityFrameworkCore;
using WebApp.Application.Interfaces;
using WebApp.Application.Viaticos.Queries.GetViaticosVigentes;
using WebApp.Domain;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class ViaticosService : IViaticosService
{
    private readonly WebAppDbContext _context;
    public ViaticosService(WebAppDbContext context)
    {
        _context = context;
    }
    public async Task<List<GetViaticosVigentesResponse>> GetViaticosVigentesAsync()
    {
        var viaticos = new List<string> { ViaticosTipos.Almuerzo, ViaticosTipos.Cena, ViaticosTipos.Desayuno, ViaticosTipos.Hospedaje };
        return _context.Viaticos.Where(v => v.Vigente==true && viaticos.Contains(v.Nombre!))
               .Select(v => new GetViaticosVigentesResponse
               {
                   Id = v.ViaticoId,
                   Nombre = v.Nombre,
                   Monto = v.Monto
               })
               .AsNoTracking()
               .ToList();
    }
}