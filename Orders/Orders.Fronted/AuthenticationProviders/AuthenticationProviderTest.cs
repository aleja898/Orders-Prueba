using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Orders.Fronted.AuthenticationProviders
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(500);
            var anonymous = new ClaimsPrincipal();
            var user = new ClaimsIdentity(authenticationType: "test");
            var admin = new ClaimsIdentity(new List<Claim>
            {
                new Claim("FirstName", "Alejandra"),
                new Claim("LastName", "Camargo"),
                new Claim(ClaimTypes.Name, "alejacamargo84@gmail.com"),
                new Claim(ClaimTypes.Role, "Admin")
            },
            authenticationType: "test");

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(admin)));
        }
    }
}
