using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WebApp.Application.Comisiones.Queries.PlanViajeExcel;
using WebApp.Application.ComisionViaticos.Queries.GetComisionViatico;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using WebApp.Infrastructure.Identity;
using WebApp.Persistence;

namespace WebApp.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly IWebHostEnvironment _env;
    private readonly IUsuarioService _usuarioService;
    private readonly IComisionService _comisionService;

    public ReportService(IWebHostEnvironment env, IUsuarioService usuarioService, IComisionService comisionService )
    {
        _env = env;
        _usuarioService = usuarioService;
        _comisionService = comisionService;
    }

    public async Task<byte[]> GetExcelPlanViajeAsync(int idUsuario, int idComision)
    {
        var planViaje = await _comisionService.GetPlanViajeResponseAsync(idUsuario, idComision);
        planViaje.Nombre = await _usuarioService.GetNombreUsuarioAsync(idUsuario);

        var filasViaticos = ConstruirFilasViaticos(planViaje);

        var path = Path.Combine(_env.ContentRootPath, "Templates", "PlanViajeTemplate.xlsx");

        using var workbook = new XLWorkbook(path);
        var sheet = workbook.Worksheet(1);

        InsertarDatosSimples(sheet, planViaje);
        InsertarTablaViaticos(sheet, filasViaticos);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }
    private List<PlanViajeFilaViatico> ConstruirFilasViaticos(PlanViajeResponse planViaje)
    {
        var resultado = new List<PlanViajeFilaViatico>();

        if (planViaje.Viaticos == null)
            return resultado;

        var fechaInicio = DateOnly.FromDateTime(planViaje.Fecha_Salida);
        var fechaFin = DateOnly.FromDateTime(planViaje.Fecha_Regreso);

        while (fechaInicio <= fechaFin)
        {
            var viaticosDelDia = planViaje.Viaticos
                .Where(v => v.Fecha == fechaInicio)
                .ToList();

            decimal? desayuno = null;
            decimal? almuerzo = null;
            decimal? cena = null;
            decimal? hospedaje = null;

            if (viaticosDelDia.Any())
            {
                desayuno = viaticosDelDia
                    .Where(v => v.Tipo_viatico == ViaticosTipos.Desayuno)
                    .Sum(v => v.Monto);

                almuerzo = viaticosDelDia
                    .Where(v => v.Tipo_viatico == ViaticosTipos.Almuerzo)
                    .Sum(v => v.Monto);

                cena = viaticosDelDia
                    .Where(v => v.Tipo_viatico == ViaticosTipos.Cena)
                    .Sum(v => v.Monto);

                hospedaje = viaticosDelDia
                    .Where(v => v.Tipo_viatico == ViaticosTipos.Hospedaje)
                    .Sum(v => v.Monto);

                if (desayuno == 0) desayuno = null;
                if (almuerzo == 0) almuerzo = null;
                if (cena == 0) cena = null;
                if (hospedaje == 0) hospedaje = null;
            }

            resultado.Add(new PlanViajeFilaViatico
            {
                Fecha = fechaInicio,
                Desayuno = desayuno,
                Almuerzo = almuerzo,
                Cena = cena,
                Hospedaje = hospedaje
            });

            fechaInicio = fechaInicio.AddDays(1);
        }

        return resultado;
    }
    private void InsertarDatosSimples(IXLWorksheet sheet, PlanViajeResponse planViaje)
    {
        sheet.Cell("B7").Value = planViaje.Departamento;
        sheet.Cell("B10").Value = planViaje.Fecha_Salida.ToShortDateString();
        sheet.Cell("B11").Value = planViaje.Fecha_Salida.ToShortTimeString();
        sheet.Cell("E10").Value = planViaje.Fecha_Regreso.ToShortDateString();
        sheet.Cell("E11").Value = planViaje.Fecha_Regreso.ToShortTimeString();
        sheet.Cell("A30").Value = planViaje.Nombre;
        sheet.Cell("A14").Value = planViaje.Descripcion;
        sheet.Cell("F26").Value = planViaje.TotalCombustibleAutorizado;
        sheet.Cell("B19").Value = planViaje.TotalDesayuno;
        sheet.Cell("C19").Value = planViaje.TotalAlmuerzo;
        sheet.Cell("D19").Value = planViaje.TotalCena;
        sheet.Cell("E19").Value = planViaje.TotalHospedaje;
        sheet.Cell("F19").Value = planViaje.TotalViaticos;
    }
    private void InsertarTablaViaticos(IXLWorksheet sheet, List<PlanViajeFilaViatico> filas)
    {
        int filaInicio = 18;

        if (filas.Count > 1)
        {
            sheet.Row(filaInicio).InsertRowsBelow(filas.Count - 1);
        }
        int row = 18;
        
        foreach (var fila in filas)
        {
            sheet.Cell(row, 1).Value = fila.Fecha.ToString("dd/MM/yyyy");
            sheet.Cell(row, 2).Value = fila.Desayuno;
            sheet.Cell(row, 3).Value = fila.Almuerzo;
            sheet.Cell(row, 4).Value = fila.Cena;
            sheet.Cell(row, 5).Value = fila.Hospedaje;
            sheet.Cell(row, 6).Value = fila.Total;
            row++;
        }
    }
}
public class PlanViajeFilaViatico
{
    public DateOnly Fecha { get; set; } 
    public decimal? Desayuno { get; set; }
    public decimal? Almuerzo { get; set; }
    public decimal? Cena { get; set; }
    public decimal? Hospedaje { get; set; }
    public decimal Total =>
        (Desayuno ?? 0) +
        (Almuerzo ?? 0) +
        (Cena ?? 0) +
        (Hospedaje ?? 0);
}