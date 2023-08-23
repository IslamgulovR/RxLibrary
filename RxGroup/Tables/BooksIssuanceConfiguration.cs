using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RxGroup.Models;

namespace RxGroup.Tables;

public class BooksIssuanceConfiguration : IEntityTypeConfiguration<BookIssuance>
{
    public void Configure(EntityTypeBuilder<BookIssuance> builder)
    {
        builder.ToTable("BooksIssuance");
    }
}