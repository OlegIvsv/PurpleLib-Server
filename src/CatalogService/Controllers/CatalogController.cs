using AutoMapper;
using CatalogService.Contracts;
using CatalogService.Data;
using CatalogService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Controllers;

[ApiController]
[Route("api/{controller}")]
public class CatalogController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly CatalogDbContext _context;

    public CatalogController(CatalogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCatalogItems()
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
    public async Task<IActionResult> GetAllCatalogItems(Guid id)
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

    [HttpPost]
    public async Task<IActionResult> CreateCatalogItem(CatalogItemCreateRequest request)
    {
        var item = _mapper.Map<CatalogItem>(request);
        await _context.CatalogItems.AddAsync(item);
        await _context.SaveChangesAsync();

        var responseItem = _mapper.Map<CatalogItemResponse>(item);
        return Ok(responseItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCatalogItem(Guid id, CatalogItemUpdateRequest request)
    {
        var item = await _context.CatalogItems
            .Include(c => c.Flora)
            .ThenInclude(f => f.Pictures)
            .FirstOrDefaultAsync(ci => ci.Id == id);

        if (item is null)
            return NotFound();

        _mapper.Map(request, item);
        _context.CatalogItems.Update(item);
        await _context.SaveChangesAsync();

        var responseItem = _mapper.Map<CatalogItemResponse>(item);
        return Ok(responseItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCatalogItem(Guid id)
    {
        var item = await _context.CatalogItems
            .Include(i => i.Flora)
            .FirstOrDefaultAsync(i => i.Id == id);
        if (item is null)
            return NotFound();
        // TODO: deleting should be implemented as cascade
        _context.Entry(item.Flora).State = EntityState.Deleted;
        _context.CatalogItems.Remove(item);
        await _context.SaveChangesAsync();

        return Ok();
    }
}