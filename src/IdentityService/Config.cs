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
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                AlwaysIncludeUserClaimsInIdToken = true

            },
            new()
            {
                ClientId = "purplelib-nextjs-client",
                ClientName = "PurpleLib Next.js Client",
                ClientSecrets = {new ("purplelib-nextjs-client_secret".Sha256())},
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RequirePkce = false,
                RedirectUris = {"http://localhost:3000/api/auth/callback/id-server"},
                AllowOfflineAccess = true,
                AllowedScopes = {"openid", "profile", "purplelib-app"},
                AccessTokenLifetime = 3600*24*30,
                AlwaysIncludeUserClaimsInIdToken = true
            }
        };  
} 