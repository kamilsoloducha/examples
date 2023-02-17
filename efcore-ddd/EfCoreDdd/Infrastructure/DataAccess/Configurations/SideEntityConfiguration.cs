using EfCoreDdd.Model.Entities;
using EfCoreDdd.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDdd.Infrastructure.DataAccess.Configurations;

public class SideEntityConfiguration : IEntityTypeConfiguration<Side>
{
    public void Configure(EntityTypeBuilder<Side> builder)
    {
        builder.ToTable("Sides", LocalDbContext.Schema);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Label).HasConversion(
            x => x.Value,
            x => new Label(x)
        );

        builder.Property(x => x.Example).HasConversion(
            x => x.Value,
            x => new Example(x)
        );
        
        builder.Property(x => x.Type);
    }
}