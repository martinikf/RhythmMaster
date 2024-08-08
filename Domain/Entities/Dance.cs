using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public sealed class Dance : Entity<DanceId>
{
    [Required, MaxLength(128)]
    public string Name { get; set; } = string.Empty;
    
    private ICollection<SongDance>? _songDances;

    public ICollection<SongDance> SongDances
    {
        get { return _songDances ??= new HashSet<SongDance>(); }
    }

    public Dance(){}

    public Dance(string name)
    {
        Name = name;
    }
    
    public Dance(DanceId id, string name) : base(id)
    {
        Name = name;
    }
}

public readonly record struct DanceId(int Value);