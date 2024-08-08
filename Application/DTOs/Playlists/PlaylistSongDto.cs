using Domain.Entities;

namespace Application.DTOs.Playlists;

public record PlaylistSongDto
(
    PlaylistId PlaylistId, 
    SongId SongId
);