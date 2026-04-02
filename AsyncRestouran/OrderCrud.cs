using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AsyncRestouran
{
    public class OrderCrud : ICrudServiceAsync<Order>
    {
        private readonly SemaphoreSlim _fileSemaphore = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public List<Order> _items = new List<Order>();
        private readonly string _filePath;
        
        public OrderCrud(string filePath = "OrderCrud.json")
        {
            _filePath = filePath;
            if (File.Exists(_filePath))
            {
                try
                {
                    string json = File.ReadAllText(_filePath);
                    _items = JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
                }
                catch (Exception)
                {
                    _items = new List<Order>();
                }
            }
            else
            {
                _items = new List<Order>();
            }
        }
        
        
        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public async Task<bool> SaveAsync()
        {
            await _fileSemaphore.WaitAsync();
            try
            {
                await _semaphore.WaitAsync();
                var itemsToSave = new List<Order>(_items);
                _semaphore.Release();

                string jsonLine = JsonSerializer.Serialize(itemsToSave, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });

                await File.WriteAllTextAsync(_filePath, jsonLine);
                return await Task.FromResult(true);
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }

        async Task<bool> ICrudServiceAsync<Order>.CreateAsync(Order element)
        {
            await _semaphore.WaitAsync();
            try
            {
                element.Id = Guid.NewGuid();
                _items.Add(element);
            }
            finally
            {
                _semaphore.Release();
            }
            return await SaveAsync();


        }

        IEnumerator<Order> IEnumerable<Order>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        async Task<IEnumerable<Order>> ICrudServiceAsync<Order>.ReadAllAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                return _items.ToList();
            }
            finally
            {
                _semaphore.Release(); 
            }
        }


        async Task<IEnumerable<Order>> ICrudServiceAsync<Order>.ReadAllAsync(int page, int amount)
        {
            await _fileSemaphore.WaitAsync();
            try
            {
                string jsonData = await File.ReadAllTextAsync("orders.json");
                var allOrders = JsonSerializer.Deserialize<List<Order>>(jsonData);

                return allOrders
                    .Skip((page - 1) * amount)
                    .Take(amount)
                    .ToList();
            }
            finally
            {
                _fileSemaphore.Release(); 
            }
        }

        async Task<Order> ICrudServiceAsync<Order>.ReadAsync(Guid id)
        {
            await Task.CompletedTask; 
            return _items.FirstOrDefault(o => o.Id == id);
        }

        async Task<bool> ICrudServiceAsync<Order>.RemoveAsync(Order element)
        {
            bool removed;
            await _semaphore.WaitAsync();
            try
            {
                removed = _items.Remove(element);
            }
            finally
            {
                _semaphore.Release(); 
            }

            if (removed)
                return await SaveAsync();

            return false;
        }

        async Task<bool> ICrudServiceAsync<Order>.UpdateAsync(Order element)
        {
            await _semaphore.WaitAsync(); 
            try
            {
                var existingOrder = _items.FirstOrDefault(o => o.Id == element.Id);

                if (existingOrder == null)
                    return false;

                var index = _items.IndexOf(existingOrder);
                _items[index] = element;
            }
            finally
            {
                _semaphore.Release(); 
            }

            return await SaveAsync(); 
        }
    }
}
