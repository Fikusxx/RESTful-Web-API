

namespace Application.Persistence.Contracts;

public interface IGenericRepository<T> where T : class
{
    public Task<T> GetAsync(int id);
    public Task<IReadOnlyList<T>> GetAllAsync();
    public Task<T> AddAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task<bool> Exists(int id);
}
