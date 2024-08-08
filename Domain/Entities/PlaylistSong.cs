namespace Domain.Entities;

public class PlaylistSong : Entity<PlaylistSongId>
{
    public Playlist Playlist { get; set; } = null!;

    public Song Song { get; set; } = null!;
    
    public int? Order { get; set; }
    
    public PlaylistSong(){}
    
    public PlaylistSong(Playlist playlist, Song song, int? order)
    {
        Playlist = playlist;
        Song = song;
        Order = order;
    }

    public PlaylistSong(PlaylistSongId id, Playlist playlist, Song song, int? order) : base(id)
    {
        Playlist = playlist;
        Song = song;
        Order = order;
    }
}

public readonly record struct PlaylistSongId(int Value);