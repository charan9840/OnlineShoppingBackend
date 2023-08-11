using Amazon.SecurityToken.Model;
using DnsClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using MongoDB.Driver;
using OnlineShoppingBackend.Data;
using OnlineShoppingBackend.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Text;

namespace OnlineShoppingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IMongoCollection<User> _User;
        private readonly IDatabaseSettings _settings;

        public AuthController(IConfiguration configuration, IDatabaseSettings settings)
        {

            _settings = settings;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _User = database.GetCollection<User>("RegisteredUser");
        }
        /// <summary>
        /// user login
        /// </summary>
        /// <param name="loginDetails"></param>
        /// <returns></returns>

        [HttpPost("Login"), AllowAnonymous]
        public IActionResult Authenticate(Login loginDetails)
        {
            var user = _User.Find(x => x.LoginId == loginDetails.loginId).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            var tokenhandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is my custom Secret key for authentication"));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.isAdmin? "admin": "user"),
            };

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
