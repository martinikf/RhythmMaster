using Domain.Enums;

namespace Application.DTOs.Playlists;

public record CreatePlaylistDto
(
    string Name, 
    string? Description, 
    PlaylistVisibility? Visibility
);