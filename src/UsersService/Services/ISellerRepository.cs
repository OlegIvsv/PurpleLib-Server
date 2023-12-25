using UsersService.Models;

namespace UsersService.Services;

public interface ISellerRepository
{
    Task CreateAsync(Seller seller);
    Task<Seller?> GetByIdAsync(Guid id);
    Task<Seller?> EditAsync(Seller seller);
}