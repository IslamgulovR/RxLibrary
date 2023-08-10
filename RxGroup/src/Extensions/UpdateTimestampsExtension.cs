using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RxGroup.Models.Interfaces;

namespace RxGroup.Extensions;

/// <summary>
///     Обновление TimeStamps
/// </summary>
public static class UpdateTimestampsExtension
{
    private const int AllowedThreshold = 10;
    
    public static void UpdateTimestamps(this EntityEntry entry)
    {
        if (entry.Entity is not ITimeStamps) return;

        var utcNow = DateTimeOffset.UtcNow;

        if (entry.State == EntityState.Added)
        {
            var createdAt = (DateTimeOffset?) entry.CurrentValues["CreatedAt"];

            if (!createdAt.HasValue || Math.Abs((utcNow - createdAt.Value).Seconds) > AllowedThreshold)
                entry.CurrentValues["CreatedAt"] = utcNow;
        }
        else
        {
            entry.Property("CreatedAt").IsModified = false;
        }

        entry.CurrentValues["UpdatedAt"] = utcNow;
    }
}