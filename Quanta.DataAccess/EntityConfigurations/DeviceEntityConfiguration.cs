﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Quanta.DataAccess.Entities;

namespace Quanta.DataAccess.EntityConfigurations
{
    public class DeviceEntityConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder
                .HasKey(o => o.Id);

            builder
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(o => o.CreatedOn)
                .ValueGeneratedOnAdd();

            builder
                .Property(o => o.LastModified)
                .ValueGeneratedOnAddOrUpdate();

            builder
                .Property(o => o.Enabled)
                .HasDefaultValue(true);

            builder
                .HasMany(o => o.DeviceAccess)
                .WithOne(o => o.Device)
                .HasForeignKey(o => o.DeviceId);
        }
    }
}