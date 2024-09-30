//using Blazored.LocalStorage;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace BlazorAppAuthStateProvider.Client
//{
//    public class CustomAuthenticationStateProvider2 : AuthenticationStateProvider
//    {
//        private readonly ILocalStorageService _localStorage;
//        private const string LocalStorageKey = "authToken";
//        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
   
//        private AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));


//        public CustomAuthenticationStateProvider2(ILocalStorageService localStorage)
//        {
//            _localStorage = localStorage;
//        }

//        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//        {
//            var token = await SafeGetTokenAsync();
//            if (string.IsNullOrEmpty(token))
//            {
//                return Anonymous;
//            }
//            var identity = new ClaimsIdentity();
//            if (!string.IsNullOrEmpty(token))
//            {
//                var jwtToken = _tokenHandler.ReadJwtToken(token);
//                if (jwtToken.ValidTo > DateTime.UtcNow)
//                {
//                    var claims = jwtToken.Claims;
//                    identity = new ClaimsIdentity(claims, "jwtAuthType");
//                }
//            }

//            var user = new ClaimsPrincipal(identity);
//            return new AuthenticationState(user);
//        }
//        private async Task<string> SafeGetTokenAsync()
//        {
//            try
//            {
//                // جلوگیری از فراخوانی LocalStorage در حالت prerendering
//                return await _localStorage.GetItemAsStringAsync(LocalStorageKey);
//            }
//            catch (InvalidOperationException)
//            {
//                // در صورتی که در prerendering باشیم و دسترسی به JavaScript interop ممنوع باشد
//                return null;
//            }
//        }
//        public void MarkUserAsAuthenticated(string token)
//        {
//            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(_tokenHandler.ReadJwtToken(token).Claims, "jwtAuthType"));
//            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
//            NotifyAuthenticationStateChanged(authState);
//        }

//        public void MarkUserAsLoggedOut()
//        {
//            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
//            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
//            NotifyAuthenticationStateChanged(authState);
//        }
//    }
//}
