//using Blazored.LocalStorage;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace BlazorAppAuthStateProvider.Client;

//public class CustomAuthenticationStateProvider : AuthenticationStateProvider
//{
//    private const string LocalStorageKey = "authToken";
//    private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
//    private readonly ILocalStorageService _localStorageService;
//    private AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

//    public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
//    {
//        _localStorageService = localStorageService;

//    }
//    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        var token = await SafeGetTokenAsync();
//        if (string.IsNullOrEmpty(token))
//        {
//            return Anonymous;
//        }
//        //string token = "";
//       // string token = await _localStorageService.GetItemAsStringAsync(LocalStorageKey)!;

//        if (string.IsNullOrEmpty(token))
//            return await Task.FromResult(new AuthenticationState(anonymous));

//        var (name, username, role) = GetClaims(token);
//        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username))
//            return await Task.FromResult(new AuthenticationState(anonymous));

//        var claims = SetClaimPrincipal(name, username, role);
//        if (claims is null)
//            return await Task.FromResult(new AuthenticationState(anonymous));
//        else
//            return await Task.FromResult(new AuthenticationState(claims));

//    }
//    public static ClaimsPrincipal SetClaimPrincipal(string name, string username, string role)
//    {
//        if (name is null || username is null) return new ClaimsPrincipal();
//        return new ClaimsPrincipal(new ClaimsIdentity(
//        [
//            new (ClaimTypes.Name,name!),
//            new ("Username",username!),
//            new ("Role",role!)
//        ], "Bearer"));
//    }
//    private static (string, string, string) GetClaims(string jwtToken)
//    {
//        try
//        {
//            if (string.IsNullOrEmpty(jwtToken)) return (null, null, null);

//            var handler = new JwtSecurityTokenHandler();
//            var token = handler.ReadJwtToken(jwtToken);

//            var name = token.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value;
//            var username = token.Claims.FirstOrDefault(x => x.Type == "Username")?.Value;
//            var role = token.Claims.FirstOrDefault(x => x.Type == "Role")?.Value;

//            return (name, username, role);
//        }
//        catch (Exception ex)
//        {

//            throw;
//        }

//    }

//    private async Task<string> SafeGetTokenAsync()
//    {
//        try
//        {
//            // جلوگیری از فراخوانی LocalStorage در حالت prerendering
//            return await _localStorageService.GetItemAsStringAsync(LocalStorageKey);
//        }
//        catch (InvalidOperationException)
//        {
//            // در صورتی که در prerendering باشیم و دسترسی به JavaScript interop ممنوع باشد
//            return null;
//        }
//    }

//    public async Task UpdateAuthenticationState(string jwtToken)
//    {
//        var claims = new ClaimsPrincipal();
//        if (!string.IsNullOrEmpty(jwtToken))
//        {

//            var (name, username, role) = GetClaims(jwtToken);
//            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username))
//                return;

//             claims = SetClaimPrincipal(name, username, role);
            
//            if (claims is null)
//                return;
//            await _localStorageService.SetItemAsStringAsync(LocalStorageKey, jwtToken);
//        }
//        else
//        {
//            await _localStorageService.RemoveItemAsync(LocalStorageKey);
//        }
//        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
//    }


//}