using Duende.IdentityServer.Models;

namespace eShop.Identity;

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
            new ApiScope("CalelogAPIScope"),
            new ApiScope("BasketAPIScope")  ,
            new ApiScope("CalelogAPIScope.read"),
            new ApiScope("CalelogAPIScope.write"),
            new ApiScope("EShoppingGateway")    

        };
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("catalogAPI", "Catalog API")
            {
                Scopes = { "CalelogAPIScope.read", "CalelogAPIScope.write" }
            }   ,
            new ApiResource("Basket", "Basket API")
            {
                Scopes = { "BasketAPIScope" }
            },
            new ApiResource("EShoppingGateway", "eShopping Gateway API") // بدون W كبيرة في النص
            {
                Scopes = { "EShoppingGateway", "BasketAPIScope" }
            }

        };
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
           
            new Client
            {
                ClientId = "catalogAPIClient",
                ClientName = "Catalog API Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("49j1A7E1-0C69-4A89-A3D6-A3ggh55998FB86B0".Sha256()) },
                AllowedScopes = { "CalelogAPIScope.read", "CalelogAPIScope.write" }
            },
            new Client
            {
                ClientId = "basketAPIClient",
                ClientName = "Basket API Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("29j1B7E1-0C69-4A89-A3D6-A3ggh55998FB86B0".Sha256()) },
                AllowedScopes = { "BasketAPIScope" }
            },
            new Client
            {
                ClientId = "eshoppinggatewayClient",
                ClientName = "eShopping Gateway Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = {new Secret("49C1A7B8-1C79-4A70-A3C6-A37998FB86B0".Sha256()) },
               
                
                AllowedScopes = { "EShoppingGateway", "BasketAPIScope" }

            }
        };
}
