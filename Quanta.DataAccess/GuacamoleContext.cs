using Microsoft.EntityFrameworkCore;
using Quanta.DataAccess.Models;

namespace Quanta.DataAccess
{
    public class GuacamoleContext : DbContext
    {
        public GuacamoleContext(DbContextOptions<GuacamoleContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(o => o.UserDevices)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<User>()
                .HasMany(o => o.DeviceAccess)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Device>()
                .Property(o => o.Enabled)
                .HasDefaultValue(true);

            modelBuilder.Entity<Device>()
                .HasMany(o => o.DeviceAccess)
                .WithOne(o => o.Device)
                .HasForeignKey(o => o.DeviceId);

            modelBuilder.Entity<DeviceAccess>()
                .HasOne(o => o.Device);

            modelBuilder.Entity<DeviceAccess>()
                .HasOne(o => o.User);

            modelBuilder.Entity<UserDevice>()
                .HasOne(o => o.Device);

            modelBuilder.Entity<UserDevice>()
                .HasOne(o => o.User);
        }

        public DbSet<Device> Devices { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<UserDevice> UserDevices { get; set; }

        public DbSet<DeviceAccess> DeviceAccess { get; set; }
    }
}
