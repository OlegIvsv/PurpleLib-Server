using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new("purplelib-app", "PurplerLib app full access"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new()
            {
                ClientId = "jetbrains",
                ClientName = "jetbrains",
                AllowedScopes = { "purplelib-app", "openid", "profile" },
                RedirectUris = { "https://youtube.com" },
                ClientSecrets = { new("jetbrains_secret".Sha256()) },
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword }
            }
        };  
}