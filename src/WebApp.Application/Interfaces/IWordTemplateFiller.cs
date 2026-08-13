using WebApp.Application.Nombramientos.Queries.NombramientoPdf;

namespace WebApp.Application.Interfaces;

public interface IWordTemplateFiller
{
    byte[] FillTemplate(string templatePath, NombramientoPdfDto nombramiento);
}