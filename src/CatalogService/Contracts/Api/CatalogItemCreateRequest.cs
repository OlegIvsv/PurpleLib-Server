using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CatalogService.Contracts.Api;

public record CatalogItemCreateRequest(
    string Title,
    string Name,
    int OriginalPrice,
    int TotalAmount,
    string? Color,
    string? Description,
    DateTime OfferEndsAt,
    DateTime? LifeTime,
    [MinLength(1)] List<string> Pictures);