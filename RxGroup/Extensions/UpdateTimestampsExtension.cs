using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RxGroup.Models.Interfaces;

namespace RxGroup.Extensions;

/// <summary>
///     Обновление TimeStamps
/// </summary>
public static class UpdateTimestampsExtension
{
    public static void UpdateTimestamps(this EntityEntry entry)
    {
        if (entry.Entity is not ITimeStamps) return;

        var entity = entry.CurrentValues;
        var utcNow = DateTimeOffset.UtcNow;

        if (entry.State == EntityState.Added)
            entity["CreatedAt"] = utcNow;
        else
            entry.Property("CreatedAt").IsModified = false;

        entity["UpdatedAt"] = utcNow;
    }
}