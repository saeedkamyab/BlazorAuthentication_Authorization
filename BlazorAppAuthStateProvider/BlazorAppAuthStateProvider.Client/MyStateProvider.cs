using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorAppAuthStateProvider.Client
{
    public class MyStateProvider : AuthenticationStateProvider
    {
        private const string LocalStorageKey = "authToken";
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        private readonly ILocalStorageService _localStorageService;

        public MyStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;

        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            //string token = "";
            string token = await _localStorageService.GetItemAsStringAsync(LocalStorageKey)!;

            if (string.IsNullOrEmpty(token))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var (fullName, username, roles) = GetClaims(token);
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var claims = SetClaimPrincipal(fullName, username, roles);
            if (claims is null)
                return await Task.FromResult(new AuthenticationState(anonymous));
            else
                return await Task.FromResult(new AuthenticationState(claims));

        }
        public static ClaimsPrincipal SetClaimPrincipal(string fullName, string username, string roles)
        {
            if (fullName is null || username is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
            [
                new ("FullName",fullName!),
            new ("Username",username!),
            new ("roles",roles!)
            ], "JwtAuth"));
        }
        private static (string, string, string) GetClaims(string jwtToken)
        {
            try
            {
                if (string.IsNullOrEmpty(jwtToken)) return (null, null, null);

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var fullName = token.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value;
                var username = token.Claims.FirstOrDefault(x => x.Type == "Username")?.Value;
                var roles = token.Claims.FirstOrDefault(x => x.Type == "Role")?.Value;

                return (fullName, username, roles);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task UpdateAuthenticationState(string jwtToken)
        {
            var claims = new ClaimsPrincipal();
            if (!string.IsNullOrEmpty(jwtToken))
            {

                var (fullName, username, roles) = GetClaims(jwtToken);
                if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username))
                    return;

                var setedClaims = SetClaimPrincipal(fullName, username, roles);
                claims = setedClaims;//I added and i don't know is true or not?
                if (setedClaims is null)
                    return;
                await _localStorageService.SetItemAsStringAsync(LocalStorageKey, jwtToken);
            }
            else
            {
                await _localStorageService.RemoveItemAsync(LocalStorageKey);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
        }


    }

}
