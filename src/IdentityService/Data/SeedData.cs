using System.Security.Claims;
using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

public static class SeedData
{
    public static void  EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (userMgr.Users.Any())
            return;

        var alice = new ApplicationUser
        {
            UserName = "alice",
            Email = "AliceSmith@email.com",
            EmailConfirmed = true,
        };
        var aliceResult = userMgr.CreateAsync(alice, "Pass123$").Result;

        if (!aliceResult.Succeeded)
            throw new Exception(aliceResult.Errors.First().Description);

        aliceResult = userMgr.AddClaimsAsync(alice,
            new Claim[]
            {
                new(JwtClaimTypes.Name, "Alice Smith"),
            }).Result;

        if (!aliceResult.Succeeded)
            throw new Exception(aliceResult.Errors.First().Description);

        Log.Debug("alice created");


        var bob = new ApplicationUser
        {
            UserName = "bob",
            Email = "BobSmith@email.com",
            EmailConfirmed = true
        };
        var bobResult = userMgr.CreateAsync(bob, "Pass123$").Result;

        if (!bobResult.Succeeded)
            throw new Exception(bobResult.Errors.First().Description);

        bobResult = userMgr.AddClaimsAsync(bob,
            new Claim[]
            {
                new(JwtClaimTypes.Name, "Bob Smith"),
            }).Result;

        if (!bobResult.Succeeded)
            throw new Exception(bobResult.Errors.First().Description);

        Log.Debug("bob created");
    }
}