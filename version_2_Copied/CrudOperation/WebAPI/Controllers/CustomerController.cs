using CrudModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : ControllerBase
    {
        private CustomerEntities.CustomerEntities _entities;

        public CustomerController(CustomerEntities.CustomerEntities entities)
        {
            _entities = entities;
        }

        [HttpGet("")]
        public string Hello()
        {
            return "Hello from Customer Controller.";
        }

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

        // login check

        [HttpGet("validCustomer/{id:int}/{password:int}")]
        public bool GetValidCustomer([FromRoute] int id, [FromRoute] int password)
        {
            // var contact =  _entities.Customer.FindAsync(id);
            //  var password = await _entities.CustPassword.FindAsync(CustomerId);

            var _password = _entities.CustPassword.FirstOrDefault(x => x.CustomerId == id);
            return password.ToString() == _password?.CPassword;



        }


        /*
        
        [HttpGet("validCustomer/{id:int}/{password:int}")]
        async Task<IActionResult> GetValidCustomer([FromRoute] int id, [FromRoute] int password)
        {
            // var contact =  _entities.Customer.FindAsync(id);
             var _password = await _entities.CustPassword.FindAsync(id);

            //  var _password = _entities.CustPassword.FirstOrDefault(x => x.CustomerId == id);
            //password.ToString() == _password?.CPassword
            //if (_password != 12345)
            //{
            //    return NotFound();
            //}
            return Ok(_password);
        }
        
       */
        // Get Method to get all the deatils of the all Customers

        [HttpGet("customer_details")]

        public async Task<IActionResult> GetAllCustomer()
        {
            return Ok(await _entities.Customer.ToListAsync());

        }


        // get Customer by id Method

        [HttpGet("customer_by_id/{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            var customer = await _entities.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        // Edit customer api get method
        [HttpGet("GetFullCustomerDetailById/{id}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int id)
        {
            var customer = await _entities.Customer.FindAsync(id);
            var address = await _entities.CustAddress.FindAsync(customer.AddressId);


            CustomerEditDetails customerEditDetails = new CustomerEditDetails();
            //  customerEditDetails.Address.Address = address.AddressText;
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
            return Ok(customerEditDetails);
        }


        // Update Cutomer Full deatils by id PUT Method
        //[HttpPut("UpdateFullCustomerDetailsById/{Id}")]
        //public async Task<IActionResult> CustomerEditDetails([FromRoute] int Id, CustomerEditDetails updateCustomerReq)
        //{
        //    var customer = await _entities.Customer.FindAsync(Id);
        //    var address = await _entities.CustAddress.FindAsync(customer.AddressId);
        //    //var customerEditDetails = await _entities.CustomerEditDetails.FindAsync(Id);
        //    if (customer != null && address != null)
        //    {
        //        address.AddressText = updateCustomerReq.Address;
        //        customer.FirstName = updateCustomerReq.FirstName;
        //        customer.LastName = updateCustomerReq.LastName;
        //        customer.AddressId = updateCustomerReq.AddressId;
        //        customer.Id = updateCustomerReq.Id;
        //        _entities.SaveChanges();
        //        return Ok(customer);
        //    }
        //    return NotFound();
        //}


        // 


        // Update customer api POST Method
        // Edit customer api get method


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


                //if (customerEditDetails == null)
                //{
                //    return NotFound();
                //}
                //await _entities.CustAddress.AddAsync(address);
                //await _entities.Customer.AddAsync(customer);

                _entities.SaveChanges();
                return Ok("Updated");

            }
            catch (Exception ex)
            {
                return Ok("Failed");
            }
        }





        //public string GetCustomerDetails()
        //{
        //    var _fname = _entities.Customer.Select(_a => _a.FirstName).ToList();
        //    var _lname = _entities.Customer.Select(_b => _b.LastName).ToList();
        //    var sb = new StringBuilder();
        //    //List<_entities.Customer> = new List<_entities.Customer> ;
        //    foreach (var fname in _fname)
        //    {
        //        sb.AppendLine(fname);
        //    }
        //    return sb.ToString();

        //}


        //[HttpPost("addCustomer")]
        //public async Task<IActionResult> AddCustomer(AddCustomer addcustomer)
        //{
        //    var Customer = new Customer()
        //    {
        //        Id = addcustomer.Id,
        //        FirstName = addcustomer.FirstName,
        //        LastName = addcustomer.LastName,
        //        AddressId = addcustomer.AddressId,
        //    };
        //    await _entities.Customer.AddAsync(Customer);
        //    await _entities.SaveChangesAsync();
        //    return Ok(Customer);
        //}

        //[HttpPut("UpdateCustomer/{Id}")]
        //public async Task<IActionResult> UpdateCustomer([FromRoute] int Id, UpdateCustomer updateCustomerReq)
        //{
        //    var customer = await _entities.Customer.FindAsync(Id);
        //    if (customer != null)
        //    {
        //        customer.FirstName = updateCustomerReq.FirstName;
        //        customer.LastName = updateCustomerReq.LastName;
        //        customer.AddressId = updateCustomerReq.AddressId;
        //        customer.Id = updateCustomerReq.Id;
        //        _entities.SaveChanges();
        //        return Ok(customer);
        //    }
        //    return NotFound();
        //}

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
