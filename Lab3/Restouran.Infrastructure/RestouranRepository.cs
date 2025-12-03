using Microsoft.EntityFrameworkCore;
using Restouran.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Infrastructure
{
    internal class RestouranRepository<T> : IRepository<T> where T: class
    {
        private readonly RestouranContext _context;
        private readonly DbSet<T> _dbSet;

        
        public RestouranRepository(RestouranContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); 
        }

        public async Task AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public Task Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
    }
}
