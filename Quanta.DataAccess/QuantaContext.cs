using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Quanta.DataAccess.Entities;
using Quanta.DataAccess.EntityConfigurations;

namespace Quanta.DataAccess
{
    public class QuantaContext : DbContext
    {
        public QuantaContext(DbContextOptions<QuantaContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DeviceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserDeviceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceAccessEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserDeviceEntityConfiguration());
        }

        public DbSet<Device> Devices { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<UserDevice> UserDevices { get; set; }

        public DbSet<DeviceAccess> DeviceAccess { get; set; }
    }
}
