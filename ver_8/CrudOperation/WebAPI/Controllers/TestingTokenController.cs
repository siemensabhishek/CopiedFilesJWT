using CustomerEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class TestingTokenController : Controller
    {
        public static CustomerEntities.CustomerEntities _entities;

        public static IConfiguration _config;

        public TestingTokenController(CustomerEntities.CustomerEntities entities, IConfiguration configuration)
        {
            _entities = entities;
            _config = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("HelloTestingTokenController")]
        public string Get()
        {
            return "Hello from Testing token Controller";
        }


        // AllowAnonymous end point to generate a token for testing purpose.
        [AllowAnonymous]
        [HttpGet("generate-Tokens-Testing")]
        public string GetTokens()
        {
            TokenController _controller = new TokenController(_entities, _config);
            CustPassword user = null;
            string token = _controller.GenerateToken(user);
            return token;
        }

    }
}
