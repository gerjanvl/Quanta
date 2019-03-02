using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Quanta.Domain.Device;

namespace Quanta.DataAccess.EntityConfigurations
{
    public class DeviceEntityConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable(nameof(Device));

            builder
                .HasKey(o => o.Id);

            builder
                .Property(o => o.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("(newid())");

            builder
                .Property(o => o.CreatedOn)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("(getdate())");

            builder
                .Property(o => o.LastModified)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("(getdate())");

            builder
                .Property(o => o.Enabled)
                .HasDefaultValue(true);

            builder
                .HasMany(o => o.Sessions)
                .WithOne(o => o.Device)
                .HasForeignKey(o => o.DeviceId);
        }
    }
}
