namespace L4D2PlayStats.Web.Models;

public class PaginationModel(int page, int totalPages, int firstItem, int lastItem, int totalItems, Func<int, string> pageUrl)
{
    private const int Window = 2;

    public int Page { get; } = page;
    public int TotalPages { get; } = totalPages;
    public int FirstItem { get; } = firstItem;
    public int LastItem { get; } = lastItem;
    public int TotalItems { get; } = totalItems;
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    public IReadOnlyCollection<int> Pages =>
    [
        .. Enumerable.Range(1, TotalPages)
            .Where(number => number == 1 || number == TotalPages || Math.Abs(number - Page) <= Window)
    ];

    public static PaginationModel Of<T>(PagedListModel<T> list, Func<int, string> pageUrl)
    {
        return new PaginationModel(list.Page, list.TotalPages, list.FirstItem, list.LastItem, list.TotalItems, pageUrl);
    }

    public string UrlFor(int number)
    {
        return pageUrl(number);
    }
}