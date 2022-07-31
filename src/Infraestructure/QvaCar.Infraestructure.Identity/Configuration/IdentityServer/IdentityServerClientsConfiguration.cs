using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
namespace QvaCar.Infraestructure.Identity.Configuration
{
    public static class IdentityServerClientsConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var provinceIdentityRes = new IdentityResource(
                        name: QvaCarClaims.Province,
                        displayName: "Province",
                        userClaims: new[] { QvaCarClaims.Province });
            var subscriptionIdentityRes = new IdentityResource(
                       name: QvaCarClaims.SubscriptionLevel,
                       displayName: "Your subsctiption level",
                       userClaims: new[] { QvaCarClaims.SubscriptionLevel });

            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                provinceIdentityRes,
                subscriptionIdentityRes,
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            var coreApiScopes = new ApiScope("qvacar.api.core", "Qva Car Main Backend");
            return new[] { coreApiScopes };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            var qvaCarCoreApiResouce = new ApiResource(
                    "qvacar.api.core",
                    "Qva Car Backend Api",
                    new string[] { QvaCarClaims.Province, QvaCarClaims.SubscriptionLevel })
            {
                Scopes = { "qvacar.api.core", },
                ApiSecrets = { new Secret("apisecret".Sha256()) }
            };
            return new[] { qvaCarCoreApiResouce };
        }

        public static IEnumerable<Client> GetClients(IHostEnvironment env, IdentityOptions options)
        {
            var mobileAppClient = new Client
            {
                ClientName = "Qva Car Mobile",
                RequireClientSecret = false,
                ClientId = "qvacar.mobile.xamarin",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                AllowOfflineAccess = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Address,
                    QvaCarClaims.Province,
                    QvaCarClaims.SubscriptionLevel,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "qvacar.api.core",
                },
                RequireConsent = false,
                RedirectUris = { "qvacarmobilexamarin://callback" },
                UpdateAccessTokenClaimsOnRefresh = true,
                AllowAccessTokensViaBrowser = true,
                PostLogoutRedirectUris = { "qvacarmobilexamarin://logout" },
            };

            var swaggerAppClient = new Client
            {
                ClientName = "Qva Car Swagger Client",
                RequireClientSecret = false,
                ClientId = "qvacar.test.swagger",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                AllowOfflineAccess = false,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Address,
                    QvaCarClaims.Province,
                    QvaCarClaims.SubscriptionLevel,
                    "qvacar.api.core",
                },
                RequireConsent = false,
                RedirectUris = { $"{options.Issuer}/swagger/oauth2-redirect.html" },
                AllowAccessTokensViaBrowser = true,
            };

            var clients = new List<Client> { mobileAppClient };
            if (!env.IsTesting())
            {
                clients.Add(swaggerAppClient);
            }

            return clients;
        }

    }
}