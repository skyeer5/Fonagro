using System.Globalization;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using WebApp.Application.Interfaces;
using WebApp.Application.Nombramientos.Queries.NombramientoPdf;

public sealed class OpenXmlWordTemplateFiller : IWordTemplateFiller
{
    private static readonly CultureInfo _culturaEs = new("es-GT");
    private static string FormatFechaLarga(DateTime fecha)
    {
        return fecha.ToString("d 'de' MMMM 'de' yyyy", _culturaEs);
    }
    public byte[] FillTemplate(string templatePath, NombramientoPdfDto dto)
    {
        using var memoryStream = new MemoryStream();
        using (var templateFile = File.OpenRead(templatePath))
        {
            templateFile.CopyTo(memoryStream);
        }

        using (var wordDoc = WordprocessingDocument.Open(memoryStream, isEditable: true))
        {
            var body = wordDoc.MainDocumentPart!.Document!.Body!;
            var placeholders = BuildPlaceholderMap(dto);

            foreach (var paragraph in body.Descendants<Paragraph>())
            {
                ReplaceInParagraph(paragraph, placeholders);
            }

            wordDoc.MainDocumentPart.Document.Save();
        }

        return memoryStream.ToArray();
    }

    private Dictionary<string, string> BuildPlaceholderMap(NombramientoPdfDto dto)
    {
        return new Dictionary<string, string>
        {
            ["{{NUMERO_NOMBRAMIENTO}}"] = dto.NumeroNombramiento,
            ["{{FECHA_NOMBRAMIENTO}}"] = FormatFechaLarga(dto.FechaCreacion),
            ["{{NOMBRE_USUARIO}}"] = dto.NombreCompleto,
            ["{{PUESTO}}"] = dto.Puesto,
            ["{{PROPOSITO}}"] = dto.Proposito,
            ["{{DESTINOS}}"] = Format(dto.Destinos),
            ["{{FECHA_SALIDA}}"] = FormatFechaLarga(dto.FechaInicio),
            ["{{FECHA_REGRESO}}"] = FormatFechaLarga(dto.FechaFin),
            ["{{EMITIDO_POR}}"] = dto.EmitidoPor,
        };
    }

    private void ReplaceInParagraph(Paragraph paragraph, Dictionary<string, string> placeholders)
    {
        var runs = paragraph.Descendants<Run>().ToList();
        if (runs.Count == 0) return;

        var fullText = string.Concat(runs.Select(r => r.InnerText));
        if (!fullText.Contains("{{")) return;

        var replacedText = fullText;
        foreach (var (key, value) in placeholders)
        {
            replacedText = replacedText.Replace(key, value ?? string.Empty);
        }

        if (replacedText == fullText) return; 

        var firstRun = runs[0];
        var textElement = firstRun.GetFirstChild<Text>();
        if (textElement is null)
        {
            textElement = new Text();
            firstRun.AppendChild(textElement);
        }
        textElement.Text = replacedText;
        textElement.Space = DocumentFormat.OpenXml.SpaceProcessingModeValues.Preserve;

        for (int i = 1; i < runs.Count; i++)
        {
            runs[i].Remove();
        }
    }
    private string Format(IReadOnlyCollection<NombramientoPdfDestinosDto> destinos)
    {
        if (destinos is null || destinos.Count == 0)
            return string.Empty;

        var grupos = destinos
            .GroupBy(d => d.Departamento)
            .Select(g => new
            {
                Departamento = g.Key,
                Municipios = g.Select(d => d.Municipio)
                              .Distinct()
                              .ToList()
            })
            .ToList();

        var partes = grupos.Select(FormatGrupo);

        return string.Join("; ", partes) + ".";
    }

    private static string FormatGrupo(dynamic grupo)
    {
        List<string> municipios = grupo.Municipios;
        string departamento = grupo.Departamento;

        string etiqueta = municipios.Count == 1 ? "municipio de" : "municipios de";
        string listaMunicipios = FormatListaConY(municipios);

        return $"{departamento}, {etiqueta} {listaMunicipios}";
    }

    private static string FormatListaConY(IReadOnlyList<string> items)
    {
        if (items.Count == 1) return items[0];
        if (items.Count == 2) return $"{items[0]} y {items[1]}";

        var todosMenosUltimo = string.Join(", ", items.Take(items.Count - 1));
        return $"{todosMenosUltimo} y {items[^1]}";
    }
}