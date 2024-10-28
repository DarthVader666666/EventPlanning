namespace EventPlanning.Bll.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity?>> GetListAsync();
        Task<TEntity?> GetAsync(object? id);
        Task<TEntity?> CreateAsync(TEntity item);
        Task<TEntity?> UpdateAsync(TEntity item);
        Task<TEntity?> DeleteAsync(object? id);
        Task<bool> ExistsAsync(TEntity item);
    }
}
