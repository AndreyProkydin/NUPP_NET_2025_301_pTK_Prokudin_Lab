using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Infrastructure
{
    public class RestouranCrudService<T> : ICrudServiceAsync<T> where T : class
    {

        private readonly IRepository<T> _repository;
        private readonly RestouranContext _context;

        public RestouranCrudService(IRepository<T> repository, RestouranContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            return await SaveAsync();
        }

        public async Task<T> ReadAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var allItems = await _repository.GetAllAsync();
            return allItems.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _repository.Delete(element);
            return await SaveAsync();
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _repository.Update(element);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }
    }
}
