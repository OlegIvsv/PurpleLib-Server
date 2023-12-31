using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services;

public class ExtendedProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ExtendedProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
       
        var existingClaims = await _userManager.GetClaimsAsync(user);
        var claims = new Claim[]
        {
            new("username", user.UserName),
        };
        
        context.IssuedClaims.AddRange(claims);
        context.IssuedClaims.AddRange(existingClaims);
    }

    public Task IsActiveAsync(IsActiveContext context)
    { 
        return Task.CompletedTask;
    }
}