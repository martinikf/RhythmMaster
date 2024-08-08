namespace Domain.Entities;

public class SongDance : Entity<SongDanceId>
{
    public Song Song { get; set; } = null!;
    
    public Dance Dance { get; set; } = null!;

    public SongDance(){}
    
    public SongDance(Song song, Dance dance)
    {
        Song = song;
        Dance = dance;
    }
    
    public SongDance(SongDanceId id, Song song, Dance dance) : base(id)
    {
        Song = song;
        Dance = dance;
    }
}

public readonly record struct SongDanceId(int Value);