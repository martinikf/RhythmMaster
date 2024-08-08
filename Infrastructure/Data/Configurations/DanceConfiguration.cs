using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class DanceConfiguration : IEntityTypeConfiguration<Dance>
{
    public void Configure(EntityTypeBuilder<Dance> builder)
    {
        builder.HasKey(p=>p.Id);
        builder.Property(p => p.Id).HasConversion(
            id => id.Value,
            value => new DanceId(value));
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
    }
}