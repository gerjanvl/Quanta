﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Quanta.DataAccess.Entities;

namespace Quanta.DataAccess.EntityConfigurations
{
    public class UserDeviceEntityConfiguration : IEntityTypeConfiguration<UserDevice>
    {
        public void Configure(EntityTypeBuilder<UserDevice> builder)
        {
            builder
                .HasKey(o => o.Id);

            builder
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(o => o.Device);

            builder
                .HasOne(o => o.User);
        }
    }
}