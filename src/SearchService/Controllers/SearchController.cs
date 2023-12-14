using Microsoft.AspNetCore.Mvc;
using SearchService.Data;

namespace SearchService.Controllers;

[ApiController]
[Route("api/{controller}")]
public class SearchController: ControllerBase
{
    private readonly IItemRepository _itemRepository;

    public SearchController(IItemRepository  itemRepository)
    {
        _itemRepository = itemRepository;
    }

    [HttpGet]
    public async Task<IActionResult> SearchForItems(string query, int page, int pageSize)
    {
        var pageItems = await _itemRepository.RunSearch(query, page, pageSize);
        return Ok(pageItems);
    }
}