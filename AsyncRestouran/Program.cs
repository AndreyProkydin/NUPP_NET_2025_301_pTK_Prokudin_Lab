using System.Diagnostics;

namespace AsyncRestouran
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ICrudServiceAsync<Order> crudService = new OrderCrud();
            const int itemsToCreate = 1000;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var creationTasks = Enumerable.Range(0, itemsToCreate)
                    .Select(i => crudService.CreateAsync(Order.CreateNew()))
                    .ToList();

                await Task.WhenAll(creationTasks);

                stopwatch.Stop();
                Console.WriteLine($"Successfully created {itemsToCreate} orders in {stopwatch.ElapsedMilliseconds} ms.");

               
                var avgPrice = crudService.Average(o => o.OrderPrice);
                var maxPrice = crudService.Max(o => o.OrderPrice);
                var minPrice = crudService.Min(o => o.OrderPrice);
                var totalItems = crudService.Count(); 

                Console.WriteLine($"--- LINQ Results (Total: {totalItems} orders) ---");
                Console.WriteLine($"Average Order Price: {avgPrice:F2} UAH");
                Console.WriteLine($"Highest Order Price: {maxPrice:F2} UAH");
                Console.WriteLine($"Lowest Order Price:  {minPrice:F2} UAH");

                Console.WriteLine("\nSaving collection to file...");
                stopwatch.Restart();

                bool saved = await crudService.SaveAsync();

                stopwatch.Stop();
                Console.WriteLine($"Collection saved: {saved} (Took {stopwatch.ElapsedMilliseconds} ms)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        
        }
    }
}
