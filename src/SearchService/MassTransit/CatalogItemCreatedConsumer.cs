using AutoMapper;
using Contracts.CatalogItem;
using MassTransit;
using SearchService.Data;
using SearchService.Models;

namespace SearchService.MassTransit;

public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
{
    private readonly IMapper _mapper;
    private readonly IItemRepository _itemRepository;

    public CatalogItemCreatedConsumer(IMapper mapper, IItemRepository itemRepository)
    {
        _mapper = mapper;
        _itemRepository = itemRepository;
    }
    
    public async Task Consume(ConsumeContext<CatalogItemCreated> context)
    {
        var item = _mapper.Map<Item>(context.Message);
        await _itemRepository.Insert(item);
    }
}