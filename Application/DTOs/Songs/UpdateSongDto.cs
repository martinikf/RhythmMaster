using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.Songs;

public record UpdateSongDto
(
    SongId Id,
    string Name,
    PersonId ArtistId,
    string? SocialLink,
    int? Bpm,
    SongPower? Power,
    int? Duration,
    int? Price,
    ICollection<DanceId> DancesIds
);