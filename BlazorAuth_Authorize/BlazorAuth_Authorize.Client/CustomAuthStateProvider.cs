using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace BlazorAuth_Authorize.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private const string LocalStorageKey = "authToken";
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        //new
        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
          //  _http = http;
            _localStorage=localStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
             string token = await _localStorage.GetItemAsStringAsync(LocalStorageKey)!;

            var identity = new ClaimsIdentity();
          

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);
            if (string.IsNullOrEmpty(token))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var (name, username, permits) = GetClaims(token);
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var claims = SetClaimPrincipal(name, username, permits);
            if (claims is null)
                return await Task.FromResult(new AuthenticationState(anonymous));
            else
                return await Task.FromResult(new AuthenticationState(claims));



          //  NotifyAuthenticationStateChanged(Task.FromResult(state));

           // return state;

       

        }
        public static ClaimsPrincipal SetClaimPrincipal(string name, string username, string permits)
        {
            if (name is null || username is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
            [
                new (ClaimTypes.Name,name!),
            new ("Username",username!),
            new ("permits",permits!)
            ], "JwtAuth"));
        }
        private static (string, string, string) GetClaims(string jwtToken)
        {
            try
            {
                if (string.IsNullOrEmpty(jwtToken)) return (null, null, null);

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var name = token.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value;
                var username = token.Claims.FirstOrDefault(x => x.Type == "Username")?.Value;
                var permits = token.Claims.FirstOrDefault(x => x.Type == "permits")?.Value;

                return (name, username, permits);
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

                var (name, username, permits) = GetClaims(jwtToken);
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username))
                    return;

                var setedClaims = SetClaimPrincipal(name, username, permits);
                claims = setedClaims;//I added and i don't know is true or not?
                if (setedClaims is null)
                    return;
                await _localStorage.SetItemAsStringAsync(LocalStorageKey, jwtToken);
            }
            else
            {
                await _localStorage.RemoveItemAsync(LocalStorageKey);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
        }

        //====================================================================================

        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    var token = await GetTokenAsync();

        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "User") };
        //        var identity = new ClaimsIdentity(claims, "jwtAuthType");
        //        var user = new ClaimsPrincipal(identity);
        //        return new AuthenticationState(user);
        //    }

        //    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        //}
        //private async Task<string> GetTokenAsync()
        //{
        // var t=   await _localStorage.GetItemAsStringAsync("token");
        //    return await Task.FromResult(t);
        //}






        //old

        //public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
        //{
        //    _localStorage = localStorage;
        //    _http = http;
        //}

        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    string token = await _localStorage.GetItemAsStringAsync("token");

        //    var identity = new ClaimsIdentity();
        //    _http.DefaultRequestHeaders.Authorization = null;

        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        //        _http.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        //    }

        //    var user = new ClaimsPrincipal(identity);
        //    var state = new AuthenticationState(user);

        //    NotifyAuthenticationStateChanged(Task.FromResult(state));

        //    return state;
        //}

        //public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        //{
        //    var payload = jwt.Split('.')[1];
        //    var jsonBytes = ParseBase64WithoutPadding(payload);
        //    var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        //    return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        //}

        //private static byte[] ParseBase64WithoutPadding(string base64)
        //{
        //    switch (base64.Length % 4)
        //    {
        //        case 2: base64 += "=="; break;
        //        case 3: base64 += "="; break;
        //    }
        //    return Convert.FromBase64String(base64);
        //}
    }
}
