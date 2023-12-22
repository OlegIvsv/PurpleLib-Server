using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersService.Contracts;
using UsersService.Models;
using UsersService.Services;

namespace UsersService.Controllers;

[ApiController]
[Route("api/{controller}")]
public class SellerController : ControllerBase
{
    private readonly ISellerRepository _sellerRepository;

    public SellerController(ISellerRepository repository)
    {
        _sellerRepository = repository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleByProviderId(Guid id)
    {
        var seller = await _sellerRepository.GetById(id);
        if (seller is null)
            return NotFound();
        return Ok(seller);
    }

    [HttpPut]
    [Authorize("SellerOnly")]
    public async Task<IActionResult> UpdateSellerRecord(EditSellerRequest request)
    {
        var seller = await _sellerRepository.GetById(request.Id);
        if (seller is null)
            return NotFound();

        seller.Name = request.Name;
        seller.Email = request.Email;
        seller.Description = request.Description;
        seller.Pictures = request.Pictures;
        
        await _sellerRepository.EditSeller(seller);
        return Ok();
    }

    [HttpPost]
    [Authorize("SellerOnly")]
    public async Task<IActionResult> CreateSellerRecord(CreateSellerRequest request)
    {
        var seller = new Seller
        {
            Id = ReadUserId(),
            Name = request.Name,
            Email = request.Email, 
            Description = request.Description,
            Pictures = request.Pictures
        };
        await _sellerRepository.CreateSeller(seller);
        return Ok();
    }

    private Guid ReadUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId);
    }
}