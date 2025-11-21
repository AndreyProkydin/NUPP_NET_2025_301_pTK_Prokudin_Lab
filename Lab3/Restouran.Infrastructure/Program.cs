using Restouran.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Restouran.Infrastructure
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var context = new RestouranContext();
            context.Database.Migrate();

            var customerRepo = new RestouranRepository<Customer>(context);
            var customerService = new RestouranCrudService<Customer>(customerRepo, context);
            var orderRepo = new RestouranRepository<Order>(context);
            var orderService = new RestouranCrudService<Order>(orderRepo, context);
            var menuItemRepo = new RestouranRepository<MenuItem>(context);
            var menuService = new RestouranCrudService<MenuItem>(menuItemRepo, context);

            Console.WriteLine("CREATE:");
            var newCustomer = new Customer { TotalSpent = 0 };
            var newOrder = new Order { Status = "Pending", OrderDate = DateTime.Now };

            var steak = new MainDish
            {
                Name = "Ribeye Steak",
                Price = 550,
                Description = "Juicy steak",
                IsSpicy = false,
                Category = "Meat",
                TypeOfDish = "Main course"
            };

            var cake = new Dessert
            {
                Name = "Cheesecake",
                Price = 150,
                Description = "Cheese dessert",
                TemperatureService = "Cold",
                TypeOfDessert = "Cake"
            };

            newOrder.customer = newCustomer;  
            newOrder.MenuItem.Add(steak);     // 1-to-M
            newOrder.MenuItem.Add(cake);      // 1-to-M

            await customerService.CreateAsync(newCustomer);
            await orderService.CreateAsync(newOrder);
            await customerService.SaveAsync();

            newCustomer.CalculateTotalSpent();
            //await customerService.UpdateAsync(newCustomer);
            await customerService.SaveAsync();

            Console.WriteLine($"Created customer with ID: {newCustomer.Id}");
            Console.WriteLine($"Created order with ID: {newOrder.Id}");
            Console.WriteLine($"Created dish: {steak.Name} (ID: {steak.IdItem})");
            Console.WriteLine($"Created dish: {cake.Name} (ID: {cake.IdItem})");
            Console.WriteLine($"Customer TotalSpent: {newCustomer.TotalSpent}");

            Console.WriteLine("\nREAD:");

            var foundCustomer = await context.Customers
                .Include(c => c.order)
                .ThenInclude(o => o.MenuItem)
                .FirstOrDefaultAsync(c => c.Id == newCustomer.Id);

            if (foundCustomer != null)
            {
                Console.WriteLine($"Found customer by ID ({newCustomer.Id}): TotalSpent = {foundCustomer.TotalSpent}");
            }

            var allMenuItems = await menuService.ReadAllAsync();
            Console.WriteLine($"Total number of items in the menu (in DB): {allMenuItems.Count()}");

            var firstPage = await menuService.ReadAllAsync(1, 1);
            Console.WriteLine($"First dish on the first page: {firstPage.First().Name}");

            Console.WriteLine("\nUPDATE:");
            steak.Price = 600;
            //await menuService.UpdateAsync(steak);
            await menuService.SaveAsync();
            Console.WriteLine($"New price for '{steak.Name}': {steak.Price}");

            foundCustomer = await context.Customers
                .Include(c => c.order)
                .ThenInclude(o => o.MenuItem)
                .FirstOrDefaultAsync(c => c.Id == newCustomer.Id);

            foundCustomer?.CalculateTotalSpent();
            if (foundCustomer != null)
            {
                //await customerService.UpdateAsync(foundCustomer);
                await customerService.SaveAsync();
                Console.WriteLine($"Updated customer TotalSpent: {foundCustomer.TotalSpent}");
            }

            Console.WriteLine("\nDELETE:");
            await menuService.RemoveAsync(cake);
            await menuService.SaveAsync();

            foundCustomer = await context.Customers
                .Include(c => c.order)
                .ThenInclude(o => o.MenuItem)
                .FirstOrDefaultAsync(c => c.Id == newCustomer.Id);

            foundCustomer?.CalculateTotalSpent();
            if (foundCustomer != null)
            {
                await customerService.UpdateAsync(foundCustomer);
                await customerService.SaveAsync();
                Console.WriteLine($"Updated customer TotalSpent after deletion: {foundCustomer.TotalSpent}");
            }

            allMenuItems = await menuService.ReadAllAsync();
            Console.WriteLine($"Total number of items in the menu (after deletion): {allMenuItems.Count()}");
        }
    }
}