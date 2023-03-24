using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Data.EntityConfiguration;

public class AuthorEntityConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(a => a.FirstName)
            .HasColumnType(EntityValidations.AuthorFirstNameSqlType);
        
        builder.Property(a => a.LastName)
            .HasColumnType(EntityValidations.AuthorLastNameSqlType);
    }
}