using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Song : Entity<SongId>
{
    [Required, MaxLength(256)]
    public string Name { get; set; } = string.Empty;

    public Person? Artist { get; set; }
    
    public string? SocialLink { get; set; } = string.Empty;
    
    public int? Bpm { get; set; }
    
    public SongPower? Power { get; set; }
    
    public int? Duration { get; set; } //In seconds //TODO: consider milliseconds
    
    public int Price { get; set; }

    private ICollection<SongDance>? _songDances;

    public virtual ICollection<SongDance> SongDances
    {
        get { return _songDances ??= new HashSet<SongDance>(); }
    }

    public Song(){}
    
    public Song(SongId id) : base(id) { }
    
    
}

public readonly record struct SongId(int Value);