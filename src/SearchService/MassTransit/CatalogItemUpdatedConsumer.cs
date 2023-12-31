using AutoMapper;
using Contracts.CatalogItem;
using MassTransit;
using SearchService.Data;
using SearchService.Models;

namespace SearchService.MassTransit;

public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
{
    private readonly IMapper _mapper;
    private readonly IItemService _itemService;

    public CatalogItemUpdatedConsumer(IMapper mapper, IItemService itemService)
    {
        _mapper = mapper;
        _itemService = itemService;
    }

    public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
    {
        var item = _mapper.Map<Item>(context.Message);
        await _itemService.Update(item);
    }
}