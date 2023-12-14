namespace SearchService.Contracts;

public class PaginationResult<T>
{
    public long Total { get; set; }
    public int Count { get; set; }
    public IList<T> Items { get; set; } = new List<T>();
}