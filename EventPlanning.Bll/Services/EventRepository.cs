using EventPlanning.Bll.Interfaces;
using EventPlanning.Data;
using EventPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventPlanning.Bll.Services
{
    public class EventRepository : IRepository<Event>
    {
        private readonly EventPlanningDbContext _dbContext;

        public EventRepository(EventPlanningDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Event?> CreateAsync(Event item)
        {
            await _dbContext.Events.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;    
        }

        public Task<Event?> DeleteAsync(object? id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Event item)
        {
            throw new NotImplementedException();
        }

        public Task<Event?> GetAsync(object? id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Event?>> GetListAsync()
        {
            return await _dbContext.Events.Include(x => x.Theme).ToListAsync();
        }

        public Task<Event?> UpdateAsync(Event item)
        {
            throw new NotImplementedException();
        }
    }
}
