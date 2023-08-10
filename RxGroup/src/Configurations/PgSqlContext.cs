using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RxGroup.Extensions;

namespace RxGroup.Configurations;

public class PgSqlContext : DbContext
{
    public PgSqlContext(DbContextOptions options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    public override int SaveChanges()
    {
        OnBeforeSaving();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSaving()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            // Обновление CreatedAt и UpdatedAt в ITimeStamps
            entry.UpdateTimestamps();
            
            if (entry.State == EntityState.Deleted)
            {
                // Установка флага Deleted вместо полного удаления из БД
                entry.SoftDelete();
            }
        }
    }
}