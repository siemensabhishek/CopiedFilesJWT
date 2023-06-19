
//using CustomerEntities.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Text;

//namespace WebAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CustTokenController : ControllerBase
//    {

//        private IConfiguration _config;


//        public CustTokenController(IConfiguration configuration)
//        {
//            _config = configuration;

//        }








//        //    [Route("authUser")]

//        /*

//        private Customer AuthenticateUser(Customer user)
//        {
//            Customer _user = null;
//            if (user.FirstName == "Steve" && user.LastName == "Pound")
//            {
//                _user = new Customer { Id = 10, FirstName = "aman", LastName = "singh", AddressId = 109 };

//            }
//            return _user;
//        }
//        */

//        private CustPassword AuthenticateUser(CustPassword user, int id, string password)
//        {
//            CustPassword _user = null;


//            if (user.CustomerId == id && user.CPassword == password)
//            {
//                _user = new CustPassword { CustomerId = 11, CPassword = "123123" };

//            }
//            return _user;
//        }






//        /*
//        private string GenerateToken(CustPassword users)
//        {
//            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
//            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);


//            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);
//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//        */


//        private string GenerateToken(CustPassword users)
//        {
//            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
//            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);


//            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);
//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }




//        /*
//        [AllowAnonymous]
//        [HttpPost]
//        public IActionResult Login(Customer user)
//        {
//            IActionResult response = Unauthorized();
//            var user_ = AuthenticateUser(user);
//            if (user_ != null)
//            {
//                var token = GenerateToken(user_);
//                response = Ok(new { token = token });
//            }
//            return response;
//        }
//        */

//        [AllowAnonymous]
//        [HttpPost("generateToken")]

//        public IActionResult Login(CustPassword user, [FromRoute] int id, [FromRoute] string password)
//        {
//            IActionResult response = Unauthorized();
//            var user_ = AuthenticateUser(user, id, password);

//            if (user_ != null)
//            {
//                var token = GenerateToken(user);
//                response = Ok(new { token = token });
//            }
//            return response;

//        }


//    }
//}

