using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Quanta.DataAccess.Entities;

namespace Quanta.DataAccess.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(o => o.Id);

            builder
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasMany(o => o.UserDevices)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            builder
                .HasMany(o => o.DeviceAccess)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

        }
    }
}
