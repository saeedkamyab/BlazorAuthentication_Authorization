using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public List<User> Users { get; set; }
        public LoginController()
        {
            Users = new List<User>{
                 new User { Id = 1, UserName = "s", Password = "s", UserRole = Role.admin },
                 new User { Id = 1, UserName = "m", Password = "m", UserRole = Role.user },
            };
        }
        [HttpPost]
        public IActionResult Login(
           [FromBody] LoginModel user)
        {
            if (user.UserName == "s" && user.Password == "s")
            {
                if (string.IsNullOrWhiteSpace(user.UserName) ||
                 string.IsNullOrWhiteSpace(user.Password))
                    return null;
                var userAccount = Users.FirstOrDefault(x => x.UserName == user.UserName & x.Password == user.Password);

                var signInCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is my custom Secret key for authentication")), SecurityAlgorithms.HmacSha256);
                var tokenOption = new JwtSecurityToken(
                   issuer: "https://localhost:7186",
                   claims: new List<Claim>
                   {
                new Claim(ClaimTypes.Name,userAccount.UserName),
                new Claim("Role",userAccount.UserRole.ToString())
                   },
                   expires: DateTime.Now.AddMinutes(30),
                   signingCredentials: signInCredentials);


                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOption);

                return Ok(tokenString);
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
