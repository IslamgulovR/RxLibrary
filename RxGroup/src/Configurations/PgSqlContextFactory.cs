using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RxGroup.Configurations;

public class PgSqlContextFactory : IDesignTimeDbContextFactory<PgSqlContext>
{
    public PgSqlContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PgSqlContext>();
        optionsBuilder.UseNpgsql("host=localhost;port=5432;database=library_rx;username=postgres;password=1234");

        return new PgSqlContext(optionsBuilder.Options);
    }
}