﻿using IdentityServer4;
using IdentityServer4.Models;

namespace Dify.Core.Application.IdentityServer;

public static class IdentityServerConfig
{
    public static IEnumerable<ApiResource> GetApis()
    {
        return new List<ApiResource>()
        {
            new ("DiFYCoreAPI", "DiFY Core API") 
            {
                Scopes = { "DiFYCoreAPI" }
            }
        };
    }

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new IdentityResource[] 
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new (CustomClaimTypes.Roles, new List<string>
            {
                CustomClaimTypes.Roles
            })
        };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new ()
            {
                ClientId = "ro.client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes =
                {
                    "DiFYCoreAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 30 * 60
            }
        };
    }
    
    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new ("DiFYCoreAPI", "DiFY Core API")
        };
    }
}
