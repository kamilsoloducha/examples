using EF.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Infrastructure.Domain
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "users");

            builder.HasKey(x => x.Id);

            builder.Property<string>("Name").HasColumnName("Name");
            builder.Property<string>("Password").HasColumnName("Password");
            builder.Property<bool>("IsActive").HasColumnName("IsActive");
        }
    }
}