    using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CatalogService.Contracts;

public record CatalogItemUpdateRequest(
    [Required] string Title,
    [Required] int OriginalPrice,
    [Required] string Seller,
    [Required] DateTime OfferEndsAt,
    [Required] string Name,
    [Required] string? Color,
    [Optional] DateTime? LifeTime,
    [Required] [MinLength(1)] List<string> Pictures);