using Microsoft.AspNetCore.Mvc;
using Restouran.Infrastructure;
using Restouran.Infrastructure.Models;
using Restouran.REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.REST.Controllers
{

    [ApiController]
    [Route("api/[controller]")] 
    public class CustomersController : ControllerBase
    {
        private readonly ICrudServiceAsync<Customer> _customerService;

        public CustomersController(ICrudServiceAsync<Customer> customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomers()
        {
            var customers = await _customerService.ReadAllAsync();

            var customerModels = customers.Select(c => new CustomerModel
            {
                Id = c.Id,
                TotalSpent = c.TotalSpent
            });

            return Ok(customerModels); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetCustomer(int id)
        {
            var customer = await _customerService.ReadAsync(id);

            if (customer == null)
            {
                return NotFound(); 
            }

            var customerModel = new CustomerModel
            {
                Id = customer.Id,
                TotalSpent = customer.TotalSpent
            };

            return Ok(customerModel); 
        }


        [HttpPost]
        public async Task<ActionResult<CustomerModel>> PostCustomer(CustomerRequestModel requestModel)
        {
            var newCustomer = new Customer
            {
                TotalSpent = requestModel.TotalSpent
            };

            await _customerService.CreateAsync(newCustomer);
            var customerModel = new CustomerModel
            {
                Id = newCustomer.Id,
                TotalSpent = newCustomer.TotalSpent
            };
            return CreatedAtAction(nameof(GetCustomer), new { id = customerModel.Id }, customerModel);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerRequestModel requestModel)
        {
            var customerToUpdate = await _customerService.ReadAsync(id);

            if (customerToUpdate == null)
            {
                return NotFound(); 
            }

            customerToUpdate.TotalSpent = requestModel.TotalSpent;

            await _customerService.UpdateAsync(customerToUpdate);

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customerToDelete = await _customerService.ReadAsync(id);

            if (customerToDelete == null)
            {
                return NotFound(); 
            }

            await _customerService.RemoveAsync(customerToDelete);

            return NoContent(); 
        }
    }

}

