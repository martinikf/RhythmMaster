using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PlaylistSongConfiguration : IEntityTypeConfiguration<PlaylistSong>
{
    public void Configure(EntityTypeBuilder<PlaylistSong> builder)
    {
        builder.HasKey(p=>p.Id);
        builder.Property(p => p.Id).HasConversion(
            id => id.Value,
            value => new PlaylistSongId(value));
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
    }
}