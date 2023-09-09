using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        public UserController(DB db)
        {
            Db = db;
        }
        public DB Db { get; }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string firstName, string lastName, string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 3)
                return StatusCode(StatusCodes.Status406NotAcceptable, "یوزرنیم باید حداقل 3 کارکتر باشد");
            if (string.IsNullOrWhiteSpace(password) || password.Length < 3)
                return StatusCode(StatusCodes.Status406NotAcceptable, "پسورد باید حداقل 3 کارکتر باشد");
            if (Db.Users.Any(c => c.UserName == userName))
                return StatusCode(StatusCodes.Status406NotAcceptable, "یوزر نیم تکراریست");
            Db.Add(new User { FirstName = firstName, LastName = lastName, UserName = userName, Password = password, Id = 0 });
            Db.SaveChanges();
            return StatusCode(StatusCodes.Status200OK, "با موفقیت انجام شد");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn(string userName, string password)
        {
            var user = Db.Users.FirstOrDefault(c => c.UserName == userName && c.Password == password);
            if (user != null)
            {

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = "http://localhost:4200/",
                    Audience = "http://localhost:4200/",
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.Date.AddDays(1),
                    Subject = new ClaimsIdentity(new List<Claim> {
                             new Claim("Id" , user.Id.ToString()),
                             new Claim("UserName" , user.UserName )
                         }),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("3333333333333333")), SecurityAlgorithms.HmacSha256Signature),
                };
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                var token = jwtTokenHandler.WriteToken(jwtToken);
                return StatusCode(StatusCodes.Status200OK, token);
            }
            return StatusCode(StatusCodes.Status404NotFound, "نام کاربری یا پسورد اشتباه است.");
        }
    }
}
