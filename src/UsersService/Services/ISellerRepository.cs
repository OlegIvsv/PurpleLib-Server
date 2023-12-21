using UsersService.Models;

namespace UsersService.Services;

public interface ISellerRepository
{
    Task CreateSeller(Seller seller);
    Task<Seller?> GetById(Guid id);
    Task<Seller?> EditSeller(Seller seller);
}