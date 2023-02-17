using EfCoreDdd.Model.Entities;
using EfCoreDdd.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDdd.Infrastructure.DataAccess.Configurations;

public class DetailsEntityConfiguration : IEntityTypeConfiguration<Details>
{
    public void Configure(EntityTypeBuilder<Details> builder)
    {
        builder.ToTable("Details", LocalDbContext.Schema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);

        builder.Property(x => x.Drawer).HasConversion(
            x => x.Correct,
            x => new Drawer(x)
        );
        builder.Property(x => x.Counter).HasConversion(
            x => x.Value,
            x => new Counter(x)
        );
        
        builder.Property(x => x.IsQuestion);
        builder.Property(x => x.NextRepeat);
        
        builder.Property(x => x.SideType);
    }
}