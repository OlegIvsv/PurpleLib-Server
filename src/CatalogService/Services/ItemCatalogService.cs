using AutoMapper;
using CatalogService.Contracts.Api;
using CatalogService.Data;
using CatalogService.Entities;
using Contracts.CatalogItem;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services;

public class ItemCatalogService : IItemCatalogService
{
    private readonly CatalogDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public ItemCatalogService(CatalogDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IList<CatalogItemResponse>> GetListAsync()
    {
        var items = await _context.CatalogItems
            .Include(ci => ci.Flora)
            .ThenInclude(f => f.Pictures)
            .AsNoTracking()
            .OrderBy(ci => ci.Title)
            .ToListAsync();
        
        var itemsResult = _mapper.Map<List<CatalogItemResponse>>(items);
        return itemsResult;
    }

    public async Task<CatalogItemResponse?> GetByIdAsync(Guid id)
    {
        var item = await _context.CatalogItems
            .Include(ci => ci.Flora)
            .ThenInclude(f => f.Pictures)
            .FirstOrDefaultAsync(ci => ci.Id == id);

        if (item is null)
            return null;
        
        var itemResult = _mapper.Map<CatalogItemResponse>(item);
        return itemResult;
    }

    public async Task<IList<CatalogItemResponse>> GetBySellerAsync(Guid sellerId)
    {
        var sellerItems = await _context.CatalogItems
            .Include(ci => ci.Flora)
            .ThenInclude(f => f.Pictures)
            .Where(ci => ci.SellerId == sellerId)
            .ToListAsync();

        var resultItems = _mapper.Map<List<CatalogItemResponse>>(sellerItems);
        return resultItems;
    }

    public async Task<CatalogItemResponse> CreateAsync(CatalogItemCreateRequest request, Guid sellerId)
    {
        var item = _mapper.Map<CatalogItem>(request);
        item.SellerId = sellerId;
        await _context.CatalogItems.AddAsync(item);

        var itemCreatedMessage = _mapper.Map<CatalogItemCreated>(item);
        await _publishEndpoint.Publish(itemCreatedMessage);

        await _context.SaveChangesAsync();

        var resultItem = _mapper.Map<CatalogItemResponse>(item);
        return resultItem;
    }

    public async Task<CatalogItemResponse> UpdateAsync(Guid id, CatalogItemUpdateRequest request)
    {
        var item = await _context.CatalogItems
            .Include(c => c.Flora)
            .ThenInclude(f => f.Pictures)
            .FirstOrDefaultAsync(ci => ci.Id == id);
        
        _mapper.Map(request, item);
        _context.CatalogItems.Update(item);

        var itemUpdatedMessage = _mapper.Map<CatalogItemUpdated>(item);
        await _publishEndpoint.Publish(itemUpdatedMessage);

        await _context.SaveChangesAsync();

        var resultItem = _mapper.Map<CatalogItemResponse>(item);
        return resultItem;
    }

    public async Task DeleteAsync(Guid id)
    {
        var item = await _context.CatalogItems
            .Include(i => i.Flora)
            .FirstOrDefaultAsync(i => i.Id == id);
        
        // TODO: deleting should be implemented as cascade
        _context.Entry(item.Flora).State = EntityState.Deleted;
        _context.CatalogItems.Remove(item);

        var itemDeletedMessage = _mapper.Map<CatalogItemDeleted>(item);
        await _publishEndpoint.Publish(itemDeletedMessage);

        await _context.SaveChangesAsync();
    }
}