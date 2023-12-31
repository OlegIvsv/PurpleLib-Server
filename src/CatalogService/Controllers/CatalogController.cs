using System.Security.Claims;
using CatalogService.Contracts.Api;
using CatalogService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers;

[ApiController]
[Route("api/{controller}")]
public class CatalogController : ControllerBase
{
    private readonly IItemCatalogService _catalogService;

    public CatalogController(IItemCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCatalogItemsAsync()
    {
        var items = await _catalogService.GetListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCatalogItemByIdAsync(Guid id)
    {
        var item = await _catalogService.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("by-seller/{sellerId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCatalogItemBySellerIdAsync(Guid sellerId)
    {
        var items = await _catalogService.GetBySellerAsync(sellerId);
        return Ok(items);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCatalogItemAsync(CatalogItemCreateRequest request)
    {
        var createResultItem = await _catalogService.CreateAsync(request, ReadUserId());
        return Ok(createResultItem);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCatalogItemAsync(Guid id, CatalogItemUpdateRequest request)
    {
        var item = await _catalogService.GetByIdAsync(id);
        if (item is null)
            return NotFound();
        if (item.SellerId != ReadUserId())
            return Forbid();
        var updateResultItem = await _catalogService.UpdateAsync(id, request);
        return Ok(updateResultItem);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCatalogItemAsync(Guid id)
    {
        var item = await _catalogService.GetByIdAsync(id);
        if (item is null)
            return NotFound();
        if (item.SellerId != ReadUserId())
            return Forbid();
        await _catalogService.DeleteAsync(id);
        return Ok();
    }

    private Guid ReadUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId);
    }
}