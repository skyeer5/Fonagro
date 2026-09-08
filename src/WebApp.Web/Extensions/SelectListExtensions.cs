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

    public static List<SelectListItem> ToSelectList<T>(
        this IEnumerable<T> source,
        Func<T, string> value,
        Func<T, string> text,
        IEnumerable<int> selectedValues)
    {
        var selectedSet = selectedValues?.Select(v => v.ToString()).ToHashSet() ?? new HashSet<string>();

        return source.Select(x => new SelectListItem
        {
            Value = value(x),
            Text = text(x),
            Selected = selectedSet.Contains(value(x))
        }).ToList();
    }
}