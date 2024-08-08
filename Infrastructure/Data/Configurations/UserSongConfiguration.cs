using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserSongConfiguration : IEntityTypeConfiguration<UserSong>
{
    public void Configure(EntityTypeBuilder<UserSong> builder)
    {
        builder.HasKey(p=>p.Id);
        builder.Property(p => p.Id).HasConversion(
            id => id.Value,
            value => new UserSongId(value));
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

    }
}