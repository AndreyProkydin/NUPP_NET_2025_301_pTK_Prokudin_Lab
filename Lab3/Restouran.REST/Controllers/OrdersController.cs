using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class OrdersController : ControllerBase
    {

        private readonly ICrudServiceAsync<Order> _orderService;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<MenuItem> _menuItemRepository;
        private readonly IRepository<Customer> _customerRepository;

        public OrdersController(
            ICrudServiceAsync<Order> orderService,
            IRepository<Order> orderRepository,
            IRepository<MenuItem> menuItemRepository,
            IRepository<Customer> customerRepository)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
            _menuItemRepository = menuItemRepository;
            _customerRepository = customerRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrders()
        {
            var orders = await _orderRepository.GetAll()
                .Include(o => o.MenuItem) 
                    .ThenInclude(mi => mi.MenuItemTags) 
                        .ThenInclude(mit => mit.Tag) 
                .ToListAsync();

            var orderModels = orders.Select(o => new OrderModel
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                Status = o.Status,
                OrderDate = o.OrderDate,
                MenuItems = o.MenuItem.Select(mi => new MenuItemModel
                {
                    IdItem = mi.IdItem,
                    Name = mi.Name,
                    Description = mi.Description,
                    Price = mi.Price,
                    Tags = mi.MenuItemTags.Select(mit => new TagModel
                    {
                        Id = mit.Tag.Id,
                        Name = mit.Tag.Name
                    }).ToList()
                }).ToList()
            });

            return Ok(orderModels); 
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrder(int id)
        {
            var order = await _orderRepository.GetAll()
                .Where(o => o.Id == id)
                .Include(o => o.MenuItem)
                    .ThenInclude(mi => mi.MenuItemTags)
                        .ThenInclude(mit => mit.Tag)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound(); 
            }

            var orderModel = new OrderModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Status = order.Status,
                OrderDate = order.OrderDate,
                MenuItems = order.MenuItem.Select(mi => new MenuItemModel
                {
                    IdItem = mi.IdItem,
                    Name = mi.Name,
                    Description = mi.Description,
                    Price = mi.Price,
                    Tags = mi.MenuItemTags.Select(mit => new TagModel
                    {
                        Id = mit.Tag.Id,
                        Name = mit.Tag.Name
                    }).ToList()
                }).ToList()
            };

            return Ok(orderModel); 
        }

        [Authorize(Roles = "Administrator, Moderator, CommonUser")]
        [HttpPost]
        public async Task<ActionResult<OrderModel>> PostOrder(OrderRequestModel requestModel)
        {
            var customer = await _customerRepository.GetByIdAsync(requestModel.CustomerId);
            if (customer == null)
            {
                return BadRequest("Customer with this ID does not exist.");
            }

            var newOrder = new Order
            {
                CustomerId = requestModel.CustomerId,
                Status = requestModel.Status,
                OrderDate = DateTime.UtcNow
            };

            var menuItems = await _menuItemRepository.GetAll()
                .Where(mi => requestModel.MenuItemIds.Contains(mi.IdItem))
                .ToListAsync();

            foreach (var item in menuItems)
            {
                newOrder.MenuItem.Add(item);
            }

            await _orderService.CreateAsync(newOrder);

            var orderModel = new OrderModel
            {
                Id = newOrder.Id,
                CustomerId = newOrder.CustomerId,
                Status = newOrder.Status,
                OrderDate = newOrder.OrderDate,
                MenuItems = menuItems.Select(mi => new MenuItemModel { IdItem = mi.IdItem, Name = mi.Name, Price = mi.Price }).ToList()
            };

            return CreatedAtAction(nameof(GetOrder), new { id = orderModel.Id }, orderModel);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderRequestModel requestModel)
        {
            var orderToUpdate = await _orderRepository.GetAll()
                .Include(o => o.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orderToUpdate == null)
            {
                return NotFound(); 
            }

            orderToUpdate.Status = requestModel.Status;
            orderToUpdate.CustomerId = requestModel.CustomerId;

            orderToUpdate.MenuItem.Clear();

            var newMenuItems = await _menuItemRepository.GetAll()
                .Where(mi => requestModel.MenuItemIds.Contains(mi.IdItem))
                .ToListAsync();

            foreach (var item in newMenuItems)
            {
                orderToUpdate.MenuItem.Add(item);
            }

            await _orderService.UpdateAsync(orderToUpdate);

            return NoContent(); 
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var orderToDelete = await _orderService.ReadAsync(id);
            if (orderToDelete == null)
            {
                return NotFound(); 
            }

            await _orderService.RemoveAsync(orderToDelete);

            return NoContent();
        }

    }
}
