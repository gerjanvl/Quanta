using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Quanta.Domain.User;

namespace Quanta.DataAccess.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder
                .HasKey(o => o.Id);

            builder
                .Property(o => o.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("(newid())");

            builder
                .HasMany(o => o.UserDevices)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            builder
                .HasMany(o => o.Sessions)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

        }
    }
}
