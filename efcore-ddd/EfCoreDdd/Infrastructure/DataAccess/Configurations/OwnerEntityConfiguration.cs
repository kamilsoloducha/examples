using EfCoreDdd.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDdd.Infrastructure.DataAccess.Configurations;

public class OwnerEntityConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.ToTable("Owners", LocalDbContext.Schema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);

        builder.Property(x => x.UserId);

        builder.HasMany(x => x.Groups).WithOne(x => x.Owner);
    }
}