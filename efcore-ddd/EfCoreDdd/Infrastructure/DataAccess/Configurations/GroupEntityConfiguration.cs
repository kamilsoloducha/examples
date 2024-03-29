using EfCoreDdd.Model.Entities;
using EfCoreDdd.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDdd.Infrastructure.DataAccess.Configurations;

public class GroupEntityConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups", LocalDbContext.Schema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasConversion(
                x => x.Value,
                x => new GroupName(x));

        builder.HasOne(x => x.Owner).WithMany(x => x.Groups);

        builder.HasMany(x => x.Cards).WithOne(x => x.Group);
    }
}