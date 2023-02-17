using EfCoreDdd.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDdd.Infrastructure.DataAccess.Configurations;

public class CardEntityConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("Cards", LocalDbContext.Schema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);
        
        builder.HasOne(x => x.Front).WithMany().HasForeignKey("FrontId");
        builder.HasOne(x => x.Back).WithMany().HasForeignKey("BackId");

        builder.HasOne(x => x.FrontDetails).WithMany().HasForeignKey("FrontDetailsId");
        builder.HasOne(x => x.BackDetails).WithMany().HasForeignKey("BackDetailsId");
    }
}