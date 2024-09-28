
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorAppAuthStateProvider.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ILocalStorageService _localStorageService;

        public CustomAuthStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string jwtTokenStr = await _localStorageService.GetItemAsStringAsync("token");
            if (jwtTokenStr == null)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
            var jwt = new JwtSecurityTokenHandler();
            var jwtReader= jwt.ReadJwtToken(jwtTokenStr);
            return new AuthenticationState(new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(jwtReader.Claims,"Bearer")
            }));
        }




        //old Codes

        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicyIsInJvbGUiOiJhZG1pbiIsImV4cCI6MTcyNzUxODM5MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE4NiJ9.wxcmgjllMIB7Vl7gaI-gOgAuLojswTTgSQ1dsX9TgYs";
        //    var identity = new ClaimsIdentity(ParseClaimsFromJwt(token),"jwt");//authorize
        //   // var identity = new ClaimsIdentity();//unauthorize
        //    var user=new ClaimsPrincipal(identity);
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
