using CrudModels;
using CustomerEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;
using System.Text;





namespace WebAPI.Controllers
{
    [ApiController]
    [Route("customer")]
    //  [Authorize]
    public class CustomerController : ControllerBase
    {

        private CustomerEntities.CustomerEntities _entities;

        private IConfiguration _config;

        static string _token = "";
        static string _refreshToken = "";


        public CustomerController(CustomerEntities.CustomerEntities entities, IConfiguration configuration)
        {
            _entities = entities;
            _config = configuration;
        }

        //   [Authorize]
        //   [AllowAnonymous]
        [HttpGet("helloCustomers")]
        public string HelloCustomer()
        {
            return "Hello form customer Controller";
        }


        //    [Authorize]
        [HttpGet("")]
        public IActionResult Hello()
        {
            Console.WriteLine("Hello from Customer Controller.");
            return Ok();
        }
        //  [Authorize]
        [HttpGet("userids")]
        public string GetAllCustomers()
        {
            var ids = _entities.Customer.Select(c => c.Id).ToList();
            var sb = new StringBuilder();
            foreach (var id in ids)
            {
                sb.AppendLine(id.ToString());
            }
            return sb.ToString();
        }



        // to check if the user is valid or not 
        [AllowAnonymous]
        [HttpGet("validCustomer/{id:int}/{password:int}")]
        public List<string> GetValidCustomer([FromRoute] int id, [FromRoute] int password)
        {
            CustPassword user = null;
            TokenController _controller = new TokenController(_entities, _config);
            List<string> tokens = new List<string>();
            var _password = _entities.CustPassword.FirstOrDefault(x => x.CustomerId == id);
            bool response = password.ToString() == _password?.CPassword;

            if (response)
            {
                _token = _controller.GenerateToken(user);
                _refreshToken = GenerateRefreshToken();
                tokens.Add(_token);
                tokens.Add(_refreshToken);
                return tokens;
            }
            return tokens;
        }




        public static DateTime Expires;

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                Expires = DateTime.Now.AddSeconds(100);
                return Convert.ToBase64String(randomNumber);
            }
        }





        // Get Method to get all the deatils of the all Customers

        [HttpGet("customer_details")]

        public async Task<IActionResult> GetAllCustomer()
        {
            return Ok(await _entities.Customer.ToListAsync());
        }


        // get Customer by id Method
        //   [Authorize]
        [HttpGet("customer_by_id/{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            var customer = await _entities.Customer.FindAsync(id);

            if (customer == null)
            {
                //return NotFound();
                return StatusCode(404, "Not a valid request.");
            }
            return Ok(customer);
        }
        //   [Authorize]
        [HttpGet("GetAllCustomerDetails")]
        public async Task<IActionResult> GetAllCustomerDetails()
        {
            List<CustomerEditDetails> customers = new List<CustomerEditDetails>();
            for (int i = 1; i < 50; i++)
            {
                var customer = await _entities.Customer.FindAsync(i);
                var address = await _entities.CustAddress.FindAsync(customer.AddressId);

                CustomerEditDetails customerEditDetails = new CustomerEditDetails();
                customerEditDetails.Address = new AddressDetails
                {
                    AddressId = address.AddressId,
                    Address = address.AddressText
                };
                customerEditDetails.Id = customer.Id;
                customerEditDetails.AddressId = customer.AddressId;
                customerEditDetails.FirstName = customer.FirstName;
                customerEditDetails.LastName = customer.LastName;
                if (customerEditDetails == null)
                {
                    return NotFound();
                }
                customers.Add(customerEditDetails);
            }
            return Ok(customers);
        }



        // Edit customer api get method
        //    [Authorize]
        [HttpGet("GetFullCustomerDetailById/{id}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int id)
        {
            var customer = await _entities.Customer.FindAsync(id);
            var address = await _entities.CustAddress.FindAsync(customer.AddressId);

            CustomerEditDetails customerEditDetails = new CustomerEditDetails();
            customerEditDetails.Address = new AddressDetails
            {
                AddressId = address.AddressId,
                Address = address.AddressText
            };
            customerEditDetails.Id = customer.Id;
            customerEditDetails.AddressId = customer.AddressId;
            customerEditDetails.FirstName = customer.FirstName;
            customerEditDetails.LastName = customer.LastName;


            if (customerEditDetails == null)
            {
                return BadRequest();
                //  return NotFound();
            }
            return Ok(customerEditDetails);
        }



        // add customer
        [HttpPost("addCustomer")]
        public async Task<IActionResult> AddCustomer(AddCustomer addcustomer)
        {
            var Customer = new Customer()
            {
                Id = addcustomer.Id,
                FirstName = addcustomer.FirstName,
                LastName = addcustomer.LastName,
                AddressId = addcustomer.AddressId,
            };
            await _entities.Customer.AddAsync(Customer);
            await _entities.SaveChangesAsync();
            return Ok(Customer);
        }

        // Update customer details
        //    [Authorize]
        [HttpPut("UpdateCustomer/{Id}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int Id, UpdateCustomer updateCustomerReq)
        {
            var customer = await _entities.Customer.FindAsync(Id);
            if (customer != null)
            {
                customer.FirstName = updateCustomerReq.FirstName;
                customer.LastName = updateCustomerReq.LastName;
                customer.AddressId = updateCustomerReq.AddressId;
                customer.Id = updateCustomerReq.Id;
                _entities.SaveChanges();
                return Ok(customer);
            }
            return NotFound();
        }




        //    [Authorize]
        [HttpPut("EditCustomerById/{id}")]
        public async Task<IActionResult> UpdateCustomerById([FromRoute] int id, CustomerEditDetails updateCustomerReq)
        {
            try
            {
                var customer = await _entities.Customer.FindAsync(id);
                var address = await _entities.CustAddress.FindAsync(customer.AddressId);

                customer.Id = updateCustomerReq.Id;
                customer.FirstName = updateCustomerReq.FirstName;
                customer.AddressId = updateCustomerReq.AddressId;
                address.AddressId = updateCustomerReq.AddressId;
                address.AddressText = updateCustomerReq.Address.Address;

                _entities.SaveChanges();
                return Ok("Updated");

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }



        // to check if refresh token is still valid
        [AllowAnonymous]
        [HttpGet("ReceivedOldToken")]
        public List<string> ReceivedOldToken()
        {
            // Receiving the custom headers.

            const string HeaderKeyName = "Custom";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerVal);
            string headerValuFromClient = headerVal.ToString();
            List<string> newlyGeneratedTokens = new List<string>();
            string previousRefreshToken = _refreshToken;
            string previousAccessToken = _token;
            CustPassword user = null;
            TokenController _controller = new TokenController(_entities, _config);
            if ((headerValuFromClient == _refreshToken) && (DateTime.Now < Expires))
            {
                _token = _controller.GenerateToken(user);
                newlyGeneratedTokens.Add(_token);
                newlyGeneratedTokens.Add(_refreshToken);
            }
            else
            {
                _refreshToken = GenerateRefreshToken();
                _token = _controller.GenerateToken(user);
                newlyGeneratedTokens.Add(_token);
                newlyGeneratedTokens.Add(_refreshToken);
            }
            return newlyGeneratedTokens;

        }


        [HttpDelete("DeleteCustomer/{Id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int Id)
        {
            var customer = await _entities.Customer.FindAsync(Id);
            if (customer != null)
            {
                _entities.Remove(customer);
                await _entities.SaveChangesAsync();
                return Ok(customer);
            }
            return NotFound();

        }




    }
}
