
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorAppAuthStateProvider.Client.AuthProvider
{
    public class CustomAuthStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = (await localStorage.GetItemAsStringAsync("authToken"));
            var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : GetClaimsIdentity(token);
            var user =new ClaimsPrincipal(identity);
            return new AuthenticationState(user);

        }
        public async Task MakeUserAuthenticated(string token)
        {
            await localStorage.SetItemAsync("authToken", token);
            var identity = GetClaimsIdentity(token);
            var user=new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
        private ClaimsIdentity GetClaimsIdentity(string token) {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken=handler.ReadJwtToken(token);
            var claims = jwtToken.Claims;
            return new ClaimsIdentity(claims);
        }
    }
}
