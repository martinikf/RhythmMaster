using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.HasKey(p=>p.Id);
        builder.Property(p => p.Id).HasConversion(
            id => id.Value,
            value => new PlaylistId(value));
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        
        builder.Property(p=>p.Visibility)
            .HasConversion<string>();
    }
}

