
//using Blazored.LocalStorage;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace BlazorAppAuthStateProvider.Client
//{
//    public class AuthProvider : AuthenticationStateProvider
//    {
//        private ILocalStorageService _localStorageService;

//        public AuthProvider(ILocalStorageService localStorageService)
//        {
//            _localStorageService = localStorageService;
//        }

//        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//        {
//            string tokenJwt = await _localStorageService.GetItemAsStringAsync("tokenStr");
//            if (string.IsNullOrEmpty(tokenJwt))
//            {
//                return new AuthenticationState(new ClaimsPrincipal());
//            }
//            var jwt = new JwtSecurityTokenHandler();
//            var jwtReader = jwt.ReadJwtToken(tokenJwt);
//            return new AuthenticationState(new ClaimsPrincipal(new[]
//            {
//                new ClaimsIdentity(jwtReader.Claims,"Bearer")
//            }
//            ));

//        }
//    }
//}
