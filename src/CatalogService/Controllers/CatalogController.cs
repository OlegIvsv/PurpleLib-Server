using System.Security.Claims;
using AutoMapper;
using CatalogService.Contracts.Api;
using CatalogService.Data;
using CatalogService.Entities;
using Contracts.CatalogItem;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Controllers;

[ApiController]
[Route("api/{controller}")]
public class CatalogController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly CatalogDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public CatalogController(CatalogDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCatalogItemsAsync()
    {
        var items = await _context.CatalogItems
            .Include(ci => ci.Flora)
            .ThenInclude(f => f.Pictures)
            .AsNoTracking()
            .OrderBy(ci => ci.Title)
            .ToListAsync();
        var responseItems = _mapper.Map<List<CatalogItemResponse>>(items);
        return Ok(responseItems);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCatalogItemByIdAsync(Guid id)
    {
        var item = await _context.CatalogItems
            .Include(ci => ci.Flora)
            .ThenInclude(f => f.Pictures)
            .FirstOrDefaultAsync(ci => ci.Id == id);

        if (item is null)
            return NotFound();

        var responseItems = _mapper.Map<CatalogItemResponse>(item);
        return Ok(responseItems);
    }

    [HttpGet("by-seller/{sellerId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCatalogItemBySellerIdAsync(Guid sellerId)
    {
        var sellerItems = await _context.CatalogItems
            .Include(ci => ci.Flora)
            .ThenInclude(f => f.Pictures)
            .Where(ci => ci.SellerId == sellerId)
            .ToListAsync();
        
        var responseItems = _mapper.Map<List<CatalogItemResponse>>(sellerItems);
        return Ok(responseItems);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCatalogItemAsync(CatalogItemCreateRequest request)
    {
        var item = _mapper.Map<CatalogItem>(request);

        item.SellerId = ReadUserId();
        await _context.CatalogItems.AddAsync(item);

        var itemCreatedMessage = _mapper.Map<CatalogItemCreated>(item);
        await _publishEndpoint.Publish(itemCreatedMessage);

        await _context.SaveChangesAsync();

        var responseItem = _mapper.Map<CatalogItemResponse>(item);
        return Ok(responseItem);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCatalogItemAsync(Guid id, CatalogItemUpdateRequest request)
    {
        var item = await _context.CatalogItems
            .Include(c => c.Flora)
            .ThenInclude(f => f.Pictures)
            .FirstOrDefaultAsync(ci => ci.Id == id);

        if (item is null)
            return NotFound();

        if (item.SellerId != ReadUserId())
            return Forbid();

        _mapper.Map(request, item);
        _context.CatalogItems.Update(item);

        var itemUpdatedMessage = _mapper.Map<CatalogItemUpdated>(item);
        await _publishEndpoint.Publish(itemUpdatedMessage);

        await _context.SaveChangesAsync();

        var responseItem = _mapper.Map<CatalogItemResponse>(item);
        return Ok(responseItem);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCatalogItemAsync(Guid id)
    {
        var item = await _context.CatalogItems
            .Include(i => i.Flora)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item is null)
            return NotFound();

        if (item.SellerId != ReadUserId())
            return Forbid();

        // TODO: deleting should be implemented as cascade
        _context.Entry(item.Flora).State = EntityState.Deleted;
        _context.CatalogItems.Remove(item);

        var itemDeletedMessage = _mapper.Map<CatalogItemDeleted>(item);
        await _publishEndpoint.Publish(itemDeletedMessage);

        await _context.SaveChangesAsync();

        return Ok();
    }

    private Guid ReadUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId);
    }
}