using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

using Quanta.DataAccess.EntityConfigurations;
using Quanta.Domain;
using Quanta.Domain.Models;

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
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SessionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserDeviceEntityConfiguration());
        }

        public DbSet<Device> Devices { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserDevice> UserDevices { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public override int SaveChanges()
        {
           var modifiedEntries = ChangeTracker
                .Entries()
                .Where(e => 
                            e.State == EntityState.Modified && 
                            e.Entity.GetType().GetInterfaces().Any(o => o == typeof(ITrackableEntity))
                      );

            foreach (var entry in modifiedEntries)
            {
                entry.Property(nameof(ITrackableEntity.LastModified)).CurrentValue = DateTime.Now;
            }

            return base.SaveChanges();
        }
    }
}
