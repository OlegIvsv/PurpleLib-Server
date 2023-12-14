using Microsoft.AspNetCore.Mvc;
using SearchService.Data;

namespace SearchService.Controllers;

[ApiController]
[Route("api/{controller}")]
public class SearchController : ControllerBase
{
    private readonly IItemRepository _itemRepository;

    public SearchController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    [HttpGet]
    public async Task<IActionResult> SearchForItems(
        string? query, 
        int? page, 
        int? pageSize,
        string? sortOrder, 
        string? sortProperty)
    {
        var pageItems = await _itemRepository.RunSearch(
            query, 
            page.GetValueOrDefault(1), 
            pageSize.GetValueOrDefault(4), 
            sortOrder ??= "desc", 
            sortProperty ??= "date");
        return Ok(pageItems);
    }
}