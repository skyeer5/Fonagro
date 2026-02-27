using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Comisiones.Queries.PlanViajeExcel;
using WebApp.Application.Interfaces;
using WebApp.Infrastructure.Identity;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly IWebHostEnvironment _env;
    private readonly WebAppDbContext _context;
    private readonly IUsuarioService _usuarioService;

    public ReportService(IWebHostEnvironment env, WebAppDbContext context, IUsuarioService usuarioService)
    {
        _env = env;
        _context = context;
        _usuarioService = usuarioService;
    }

    public async Task<byte[]> GetExcelPlanViajeAsync(int idUsuario, int idComision)
    {
        var planViaje = await _context.Comisiones
            .Where(p => p.ComisionId == idComision)
            .Select(p => new PlanViajeResponse
            {
                Departamento = p.Departamento,
                Fecha_Salida = p.Fecha_Salida,
                Fecha_Regreso = p.Fecha_Regreso,
                Descripcion = p.ComisionUsuarios!.FirstOrDefault(cu => cu.UsuarioId == idUsuario)!.Descripcion
                })
            .FirstOrDefaultAsync();
        if(planViaje == null)
        {
            throw new Exception("Plan de viaje no encontrado");
        }
        planViaje.Nombre = await _usuarioService.GetNombreUsuarioAsync(idUsuario);

        
        
        var path = Path.Combine(_env.ContentRootPath, "Templates", "PlanViajeTemplate.xlsx");
        var memory = new MemoryStream(File.ReadAllBytes(path));

        using (var document = SpreadsheetDocument.Open(memory, true))
        {
            RemplazarDatosSimples(document, planViaje);
            document.WorkbookPart!.Workbook!.Save();
        }
        memory.Position = 0;
        
        return memory.ToArray();
    }


    private void RemplazarDatosSimples(SpreadsheetDocument document, PlanViajeResponse planViaje)
    {
        RemplazarTexto(document, "{{Departamento}}", planViaje.Departamento ?? string.Empty);
        RemplazarTexto(document, "{{Fecha_Salida}}", planViaje.Fecha_Salida.ToShortDateString());
        RemplazarTexto(document, "{{Hora_Salida}}", planViaje.Fecha_Salida.ToShortTimeString());
        RemplazarTexto(document, "{{Fecha_Regreso}}", planViaje.Fecha_Regreso.ToShortDateString());
        RemplazarTexto(document, "{{Hora_Regreso}}", planViaje.Fecha_Regreso.ToShortTimeString());
        RemplazarTexto(document, "{{Descripcion}}", planViaje.Descripcion ?? string.Empty);
        RemplazarTexto(document, "{{Nombre}}", planViaje.Nombre ?? string.Empty);

    }
    private void RemplazarTexto(SpreadsheetDocument document, string placeholder, string value)
    {
        var sharedStringPart = document.WorkbookPart!.SharedStringTablePart;
        if (sharedStringPart == null) return;

        var table = sharedStringPart.SharedStringTable;

        foreach (var item in table!.Elements<SharedStringItem>())
        {
            var texto = item.InnerText;

            if (!string.IsNullOrWhiteSpace(texto) && texto.Contains(placeholder))
            {
                Console.WriteLine($"Remplazando {placeholder} por {value}");
                item.RemoveAllChildren();
                item.AppendChild(new Text(texto.Replace(placeholder, value)));
            }
        }

        table.Save();
    }
}