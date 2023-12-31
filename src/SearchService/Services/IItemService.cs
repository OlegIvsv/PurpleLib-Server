using SearchService.Contracts;
using SearchService.Models;

namespace SearchService.Data;

public interface IItemService
{
    Task<PaginationResult<Item>> RunSearch(
        string? query,
        int page,
        int pageSize,
        string sortOrder,
        string sortProperty);

    Task Insert(Item item);
    Task Update(Item item);
    Task Delete(Guid id);
}