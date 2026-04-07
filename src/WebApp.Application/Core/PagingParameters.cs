namespace WebApp.Application.Core;

public abstract class PagingParameters
{
    public int PageNumber {get;set;} = 1;
    public const int MaxSize = 50;
    private int _pageSize = 15;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxSize) ? MaxSize : value;
    }
    public string? OrderBy { get; set; }
    public bool? OrderAsc { get; set; }
}