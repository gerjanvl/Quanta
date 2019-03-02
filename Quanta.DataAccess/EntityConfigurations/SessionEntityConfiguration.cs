using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Quanta.Domain.Models;

namespace Quanta.DataAccess.EntityConfigurations
{
    public class SessionEntityConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(nameof(Session));

            builder
                .HasKey(o => o.Id);

            builder
                .Property(o => o.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("(newid())");

            builder
                .HasOne(o => o.Device);

            builder
                .HasOne(o => o.User);

            builder
                .Property(o => o.CreatedOn)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("(getdate())"); 
        }
    }
}
