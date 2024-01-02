using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace HealthInsurance.Client.Helpers
{
    public class GraphAPIAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public GraphAPIAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { "https://localhost:7082/" },
                scopes: new[] { "https://localhost:7082/User.Read", "https://localhost:7082/Mail.Read" });
        }
    }
}
