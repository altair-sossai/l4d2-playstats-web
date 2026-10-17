namespace L4D2PlayStats.Web.Models;

public class PagedListModel<T>
{
    public PagedListModel(IReadOnlyCollection<T> source, int page, int pageSize)
    {
        PageSize = pageSize;
        TotalItems = source.Count;
        TotalPages = Math.Max(1, (int)Math.Ceiling(TotalItems / (double)pageSize));
        Page = Math.Clamp(page, 1, TotalPages);
        Items = [.. source.Skip((Page - 1) * pageSize).Take(pageSize)];
    }

    public IReadOnlyCollection<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalItems { get; }
    public int TotalPages { get; }
    public int FirstItem => TotalItems == 0 ? 0 : (Page - 1) * PageSize + 1;
    public int LastItem => Math.Min(Page * PageSize, TotalItems);
}