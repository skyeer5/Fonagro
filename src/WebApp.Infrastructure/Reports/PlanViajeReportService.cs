using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Hosting;
using WebApp.Application.Comisiones.Queries.PlanViajePdf;
using WebApp.Application.Interfaces;
using WebApp.Domain;

namespace WebApp.Infrastructure.Reports;

public class PlanViajeReportService : IPlanViajeReportService
{
    private readonly IWebHostEnvironment _env;
    public PlanViajeReportService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public byte[] GetExcelPlanViaje(PlanViajeDto planViaje)
    {
        var filasViaticos = ConstruirFilasViaticos(planViaje);
        var filaDestinos = ConstruirFilasDestinos(planViaje);
        planViaje.TotalCombustible = filaDestinos.Sum(d => d.Total);

        var path = Path.Combine(_env.WebRootPath, "Templates", "PlanViajeTemplate.xlsx");

        using var workbook = new XLWorkbook(path);
        var sheet = workbook.Worksheet(1);

        InsertarDatosSimples(sheet, planViaje);
        var filaViaticos = InsertarTablaViaticos(sheet, filasViaticos);
        InsertarTablaDestinos(sheet, filaDestinos, filaViaticos);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }
    private List<PlanViajeFilaViatico> ConstruirFilasViaticos(PlanViajeDto planViaje)
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
    private List<PlanViajeFilaDestinos> ConstruirFilasDestinos(PlanViajeDto planViaje)
    {
        var resultado = new List<PlanViajeFilaDestinos>();

        if (planViaje.Destinos == null)
            return resultado;

        foreach (var destino in planViaje.Destinos)
        {
            resultado.Add(new PlanViajeFilaDestinos
            {
                Descripcion = destino.Descripcion,
                Kms = destino.Kilometros,
                Galones = destino.Galones,
                PrecioGalon = planViaje.Precio_Galon
            });
        }

        return resultado;
    }
    private void InsertarDatosSimples(IXLWorksheet sheet, PlanViajeDto planViaje)
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

        sheet.Cell("B24").Value = planViaje.TotalKms;
        sheet.Cell("C24").Value = planViaje.TotalGalones;
        sheet.Cell("D24").Value = planViaje.Precio_Galon;
        sheet.Cell("E24").Value = planViaje.TotalCombustible;

        if(planViaje.Es_Gasolina)
        {
            sheet.Cell("D21").Value = "X";
        }
        else
        {
            sheet.Cell("F21").Value = "X";
        }
    }
    private int InsertarTablaViaticos(IXLWorksheet sheet, List<PlanViajeFilaViatico> filas)
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
        return row;
    }
    private void InsertarTablaDestinos(IXLWorksheet sheet, List<PlanViajeFilaDestinos> filas, int filaInicio)
    {
        filaInicio += 4;

        if (filas.Count > 1)
        {
            sheet.Row(filaInicio).InsertRowsBelow(filas.Count - 1);
        }
        int row = filaInicio;
        
        foreach (var fila in filas)
        {
            sheet.Cell(row, 1).Value = fila.Descripcion;
            sheet.Cell(row, 2).Value = fila.Kms;
            sheet.Cell(row, 3).Value = fila.Galones;
            sheet.Cell(row, 4).Value = fila.PrecioGalon;
            sheet.Cell(row, 5).Value = fila.Total;
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
public class PlanViajeFilaDestinos
{
    public string? Descripcion { get; set; } 
    public decimal? Kms { get; set; }
    public decimal? Galones { get; set; }
    public decimal? PrecioGalon { get; set; }
    public decimal Total =>
        (Galones ?? 1) * (PrecioGalon ?? 1);
}