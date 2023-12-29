using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CatalogService.Contracts.Api;

public record CatalogItemUpdateRequest(
    string Title,
    int OriginalPrice,
    string Seller,
    DateTime OfferEndsAt,
    string Name,
    string? Color,
    string? Description,
    DateTime? LifeTime,
    [MinLength(1)] List<string> Pictures);