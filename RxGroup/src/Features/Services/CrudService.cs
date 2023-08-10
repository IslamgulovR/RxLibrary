using Microsoft.EntityFrameworkCore;
using RxGroup.Configurations;

namespace RxGroup.Features.Services;

public abstract class CrudService<TEntity> where TEntity : class
{
    protected readonly PgSqlContext Database;

    protected CrudService(PgSqlContext database)
    {
        Database = database;
    }

    public virtual async Task<List<TEntity>> GetAsync()
    {
        return await Database.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();
    }
    
    public virtual async Task<TEntity> GetAsync(Guid id)
    {
        var entry = await Database.Set<TEntity>()
            .FindAsync(id);

        if (entry == null)
            throw new Exception($"Entity with Id {id} not found");

        return entry;
    }
    
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        var entry = await Database.Set<TEntity>()
            .AddAsync(entity);
        await Database.SaveChangesAsync();

        return entry.Entity;
    }

    public virtual async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
        var entry = await Database.Set<TEntity>()
            .FindAsync(id);

        if (entry == null)
            throw new Exception($"Entity with Id {id} not found");
        
        Database.Entry(entry).CurrentValues.SetValues(entity);
        await Database.SaveChangesAsync();

        return entry;
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entry = await Database.Set<TEntity>()
            .FindAsync(id);
        
        if (entry == null)
            throw new Exception($"Entity with Id {id} not found");

        Database.Remove(entry);
        await Database.SaveChangesAsync();
    }
}