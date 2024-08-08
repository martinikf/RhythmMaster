using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.Playlists;

public record UpdatePlaylistVisibilityDto
(
    PlaylistId PlaylistId,
    PlaylistVisibility Visibility
);