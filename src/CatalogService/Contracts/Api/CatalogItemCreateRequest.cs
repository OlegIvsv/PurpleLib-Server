using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CatalogService.Contracts.Api;

public record CatalogItemCreateRequest(
    [Required] string Title,
    [Required] int OriginalPrice,
    [Required] int TotalAmount,
    [Required] Guid SellerId,
    [Required] DateTime OfferEndsAt,
    [Required] string Name,
    [Required] string? Color,
    [Optional] DateTime? LifeTime,
    [Required] [MinLength(1)] List<string> Pictures);