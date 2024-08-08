using Domain.Enums;

namespace Domain.Entities;

public class Playlist : Entity<PlaylistId>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
    
    public AppUser Owner { get; set; } = null!;

    public PlaylistVisibility Visibility { get; set; } = PlaylistVisibility.Personal;

    //Cover image?

    public ICollection<Song> Songs { get; set; } = new List<Song>();

    public Playlist() { }
    
    public Playlist(string name, PlaylistVisibility visibility, AppUser owner, string? description)
    {
        Name = name;
        Description = description;
        Owner = owner;
        Visibility = visibility;
    }

    public Playlist(PlaylistId id, string name, PlaylistVisibility visibility, AppUser owner, string? description) : base(id)
    {
        Name = name;
        Description = description;
        Owner = owner;
        Visibility = visibility;
    }
    
}

public readonly record struct PlaylistId(int Value);