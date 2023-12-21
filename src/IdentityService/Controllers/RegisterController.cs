using System.Security.Claims;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[ApiController]
[Route("{controller}")]
public class RegisterController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserRequest request)
    {
        var user = new ApplicationUser()
        {
            UserName = request.Username,
            Email = request.Email,
            EmailConfirmed = true
        };

        var registerResult = await _userManager.CreateAsync(user, request.Password);
        if (registerResult.Succeeded)
        {
            await _userManager.AddClaimsAsync(user,
                new Claim[]
                {
                    new(JwtClaimTypes.Name, request.Fullname),
                    new(JwtClaimTypes.Role, request.Role.ToString().ToLower())
                });
        }
        else
        {
            return BadRequest(registerResult.Errors);
        }

        return Ok();
    }
}

public class RegisterUserRequest
{
    public UserRoles Role { get; set; }
    public string Username { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public enum UserRoles
{
    Seller = 1,
    Customer = 2
}