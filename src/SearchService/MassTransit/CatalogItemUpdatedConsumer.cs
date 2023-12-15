using AutoMapper;
using Contracts.CatalogItem;
using MassTransit;
using SearchService.Data;
using SearchService.Models;

namespace SearchService.MassTransit;

public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
{
    private readonly IMapper _mapper;
    private readonly IItemRepository _itemRepository;

    public CatalogItemUpdatedConsumer(IMapper mapper, IItemRepository itemRepository)
    {
        _mapper = mapper;
        _itemRepository = itemRepository;
    }

    public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
    {
        var item = _mapper.Map<Item>(context.Message);
        await _itemRepository.Update(item);
    }
}