using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Data.EntityConfiguration;

public class BookEntityConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(b => b.Title)
            .HasColumnType(EntityValidations.BookTitleSqlType);

        builder.Property(b => b.Description)
            .HasColumnType(EntityValidations.BookDescriptionSqlType);

        builder.Property(b => b.Price)
            .HasColumnType(EntityValidations.BookPriceSqlType);
        
    }
}