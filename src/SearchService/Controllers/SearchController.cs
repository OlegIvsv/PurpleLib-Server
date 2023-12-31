using Microsoft.AspNetCore.Mvc;
using SearchService.Data;

namespace SearchService.Controllers;

[ApiController]
[Route("api/{controller}")]
public class SearchController : ControllerBase
{
    private readonly IItemService _itemService;

    public SearchController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> SearchForItems(
        string? query, 
        int? page, 
        int? pageSize,
        string? sortOrder, 
        string? sortProperty)
    {
        var pageItems = await _itemService.RunSearch(
            query, 
            page.GetValueOrDefault(1), 
            pageSize.GetValueOrDefault(4), 
            sortOrder ??= "desc", 
            sortProperty ??= "date");
        return Ok(pageItems);
    }
}