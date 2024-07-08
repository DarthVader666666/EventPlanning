using EventPlanning.Bll.Interfaces;
using EventPlanning.Data;
using EventPlanning.Data.Entities;

namespace EventPlanning.Bll.Services
{
    public class UserRepository : IRepository<User>
    {
        private readonly EventPlanningDbContext _dbContext;

        public UserRepository(EventPlanningDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> CreateAsync(User item)
        {
            _dbContext.Users.Add(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public Task<User?> DeleteAsync(object? id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(User item)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetAsync(object? creds)
        {
            if (creds == null)
            {
                return Task.FromResult<User?>(null);
            }

            var credentials = (Tuple<string?, string?>?)creds;
            var user = _dbContext.Users.Any() 
                ? _dbContext.Users.FirstOrDefault(x => x.Password == credentials!.Item1 && x.Email == credentials.Item2)
                : null;

            return Task.Run(() => user);
        }

        public Task<IEnumerable<User?>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> UpdateAsync(User item)
        {
            throw new NotImplementedException();
        }
    }
}
