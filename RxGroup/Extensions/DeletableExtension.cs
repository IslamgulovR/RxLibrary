using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RxGroup.Models.Interfaces;

namespace RxGroup.Extensions;

public static class DeletableExtension
{
    public static void SoftDelete(this EntityEntry entry)
    {
        if (entry.Entity is not IDeletable) return;

        entry.State = EntityState.Modified;
        entry.CurrentValues["Deleted"] = true;
    }
}