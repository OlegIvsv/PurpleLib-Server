using System.Security.Claims;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public SellerController(ISellerRepository repository, IMapper mapper)
    {
        _sellerRepository = repository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSellerByIdAsync(Guid id)
    {
        var seller = await _sellerRepository.GetByIdAsync(id);
        
        if (seller is null)
            return NotFound();
        
        var response = _mapper.Map<GetSellerResponse>(seller);
        return Ok(response);
    }

    [HttpPut]
    [Authorize("SellerOnly")]
    public async Task<IActionResult> UpdateSellerRecord(EditSellerRequest request)
    {
        var seller = await _sellerRepository.GetByIdAsync(request.Id);
       
        if (seller is null)
            return NotFound();

        seller = _mapper.Map(request, seller);
        await _sellerRepository.EditAsync(seller);
        
        return Ok();
    }

    [HttpPost]
    [Authorize("SellerOnly")]
    public async Task<IActionResult> CreateSellerRecord(CreateSellerRequest request)
    {
        var seller = _mapper.Map<Seller>(request);
        seller.Id = ReadUserId();
        
        await _sellerRepository.CreateAsync(seller);
        
        return Ok();
    }

    private Guid ReadUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId);
    }
}