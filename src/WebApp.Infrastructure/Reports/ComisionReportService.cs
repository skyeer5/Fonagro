using ClosedXML.Excel;
using WebApp.Application.Comisiones.Queries.GetComisionesExcel;
using WebApp.Application.Interfaces;

namespace WebApp.Infrastructure.Reports;

public class ComisionReportService : IComisionReportService
{
    public byte[] GetExcelComisiones(List<GetComisionesExcelDto> comisiones, CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Reporte de Comisiones");

        // 1. Estilos del Encabezado
        var headers = new[]
        {
            "ID Comisión", "Fecha Creación", "Fecha Salida", "Fecha Regreso", "Estado",
            "Departamentos y Municipios", "Destinos", "Vehículo", "Nombrados / Personal",
            "Resp. Vehículo", "Creador", "Aprobador Combustible",
            "Presup. Estimado (Q)", "Presup. Aprobado (Q)", "Precio Combustible (Q)"
        };

        for (int col = 0; col < headers.Length; col++)
        {
            var cell = worksheet.Cell(1, col + 1);
            cell.Value = headers[col];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E78"); // Azul Institucional
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        }

        // 2. Llenado de Filas
        int row = 2;
        foreach (var item in comisiones)
        {
            // Formatear listas anidadas con saltos de línea (\n)
            string deptosYMuns = string.Join("\n", item.DepartamentosYMunicipios
                .Select(d => $"• {d.Departamento}: {d.Municipio}"));

            string destinos = string.Join("\n", item.Destinos.Select(d => $"• {d}"));
            string nombrados = string.Join("\n", item.Nombrados.Select(n => $"• {n}"));

            worksheet.Cell(row, 1).Value = item.ComisionId;
            worksheet.Cell(row, 2).Value = item.Fecha_Creacion_Comision;
            worksheet.Cell(row, 3).Value = item.Fecha_Salida;
            worksheet.Cell(row, 4).Value = item.Fecha_Regreso;
            worksheet.Cell(row, 5).Value = item.Estado;
            worksheet.Cell(row, 6).Value = deptosYMuns;
            worksheet.Cell(row, 7).Value = destinos;
            worksheet.Cell(row, 8).Value = item.Vehiculo;
            worksheet.Cell(row, 9).Value = nombrados;
            worksheet.Cell(row, 10).Value = item.NombreResponsableVehiculo;
            worksheet.Cell(row, 11).Value = item.NombreCreadorComision;
            worksheet.Cell(row, 12).Value = item.NombreAprobadorCombustible;
            worksheet.Cell(row, 13).Value = item.PresupuestoCombustibleEstimado;
            worksheet.Cell(row, 14).Value = item.PresupuestoCombustibleAprobado;
            worksheet.Cell(row, 15).Value = item.PrecioCombustible;

            // Formato de Fechas
            worksheet.Cell(row, 2).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
            worksheet.Cell(row, 3).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
            worksheet.Cell(row, 4).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";

            // Formato de Monedas
            worksheet.Cell(row, 13).Style.NumberFormat.Format = "\"Q\"#,##0.00";
            worksheet.Cell(row, 14).Style.NumberFormat.Format = "\"Q\"#,##0.00";
            worksheet.Cell(row, 15).Style.NumberFormat.Format = "\"Q\"#,##0.00";

            row++;
        }

        // 3. Ajustes Globales de Formato
        var dataRange = worksheet.Range(1, 1, row - 1, headers.Length);

        // Bordes finos
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.OutsideBorderColor = XLColor.LightGray;
        dataRange.Style.Border.InsideBorderColor = XLColor.LightGray;

        // Alineación vertical arriba y ajuste de texto para listas
        dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
        worksheet.Columns(6, 7).Style.Alignment.WrapText = true; // Departamentos y Destinos
        worksheet.Column(9).Style.Alignment.WrapText = true;      // Nombrados

        // Auto-ajustar ancho de columnas
        worksheet.Columns().AdjustToContents();

        // 4. Convertir a Array de Bytes para la descarga
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    
    }
}