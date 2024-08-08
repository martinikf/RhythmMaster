using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class SongDanceConfiguration : IEntityTypeConfiguration<SongDance>
{
    public void Configure(EntityTypeBuilder<SongDance> builder)
    {
        builder.HasKey(p=>p.Id);
        builder.Property(p => p.Id).HasConversion(
            id => id.Value,
            value => new SongDanceId(value));
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

    }
}