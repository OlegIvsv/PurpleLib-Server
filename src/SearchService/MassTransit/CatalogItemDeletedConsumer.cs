using AutoMapper;
using Contracts.CatalogItem;
using MassTransit;
using SearchService.Data;

namespace SearchService.MassTransit;

public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
{
    private readonly IMapper _mapper;
    private readonly IItemService _itemService;

    public CatalogItemDeletedConsumer(IMapper mapper, IItemService itemService)
    {
        _mapper = mapper;
        _itemService = itemService;
    }

    public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
    {
        var itemId = context.Message.Id;
        await _itemService.Delete(itemId);
    }
}