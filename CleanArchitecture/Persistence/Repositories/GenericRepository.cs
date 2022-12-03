using Application.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;


public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext db;

    public GenericRepository(ApplicationDbContext db)
    {
        this.db = db;
    }

    public async Task<T> AddAsync(T entity)
    {
        await db.AddAsync(entity);
        await db.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        db.Set<T>().Remove(entity);
        await db.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id)
    {
        var entity = await GetAsync(id);
        var exists = entity != null;

        return exists;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        var entities = await db.Set<T>().ToListAsync();

        return entities;
    }

    public async Task<T> GetAsync(int id)
    {
        var entity = await db.Set<T>().FindAsync(id);

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        db.Set<T>().Update(entity);
        db.Entry(entity).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }
}
