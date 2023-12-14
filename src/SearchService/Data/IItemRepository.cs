using SearchService.Contracts;
using SearchService.Models;

namespace SearchService.Data;

public interface IItemRepository
{
    Task<PaginationResult<Item>> RunSearch(string query, int page, int pageSize);
}