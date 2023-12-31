using CatalogService.Contracts.Api;

namespace CatalogService.Services;

public interface IItemCatalogService
{
    public Task<IList<CatalogItemResponse>> GetListAsync();
    public Task<CatalogItemResponse?> GetByIdAsync(Guid id);
    public Task<IList<CatalogItemResponse>> GetBySellerAsync(Guid sellerId);
    public Task<CatalogItemResponse> CreateAsync(CatalogItemCreateRequest request, Guid sellerId);
    public Task<CatalogItemResponse> UpdateAsync(Guid id, CatalogItemUpdateRequest request);
    public Task DeleteAsync(Guid id);
}