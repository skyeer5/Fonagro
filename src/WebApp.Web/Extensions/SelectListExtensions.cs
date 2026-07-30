using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Web.Extensions;

public static class SelectListExtensions
{
    public static List<SelectListItem> ToSelectList<T>(
        this IEnumerable<T> source,
        Func<T, string> value,
        Func<T, string> text)
    {
        return source.Select(x => new SelectListItem
        {
            Value = value(x),
            Text = text(x)
        }).ToList();
    }
}