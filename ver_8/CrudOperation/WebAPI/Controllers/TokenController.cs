using CustomerEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebAPI.Controllers
{
    public class TokenController : Controller
    {

        public static CustomerEntities.CustomerEntities _entities;

        public static IConfiguration _config;

        public TokenController(CustomerEntities.CustomerEntities entities, IConfiguration configuration)
        {
            _entities = entities;
            _config = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("tokenController")]
        public string HelloRefreshToken()
        {
            return "Hello from refresh token";
        }




        [AllowAnonymous]
        [HttpGet("generate-Tokens")]
        public string GetTokens()
        {
            CustPassword user = null;
            string token = GenerateToken(user);
            return token;
        }



        public string GenerateToken(CustPassword users)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null, DateTime.Now, expires: DateTime.Now.AddSeconds(Convert.ToDouble(_config["Jwt:valid"])), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
