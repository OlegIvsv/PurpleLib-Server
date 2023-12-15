using AutoMapper;
using Contracts.CatalogItem;
using MassTransit;
using SearchService.Data;

namespace SearchService.MassTransit;

public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
{
    private readonly IMapper _mapper;
    private readonly IItemRepository _itemRepository;

    public CatalogItemDeletedConsumer(IMapper mapper, IItemRepository itemRepository)
    {
        _mapper = mapper;
        _itemRepository = itemRepository;
    }

    public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
    {
        var itemId = context.Message.Id;
        await _itemRepository.Delete(itemId);
    }
}