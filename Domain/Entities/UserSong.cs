using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class UserSong : Entity<UserSongId>
{
    [Required]
    public AppUser AppUser { get; set; } = null!;
    
    [Required]
    public Song Song { get; set; } = null!;
    
    public UserSong(){}
    
    public UserSong(AppUser user, Song song)
    {
        AppUser = user;
        Song = song;
    }
    
    public UserSong(UserSongId id, AppUser user, Song song) : base(id)
    {
        AppUser = user;
        Song = song;
    }
}

public readonly record struct UserSongId(int Value);