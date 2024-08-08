using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Songs;

public record CreateSongDto
(
    string Name,
    IFormFile File,
    PersonId ArtistId,
    string? SocialLink,
    int? Bpm,
    SongPower Power,
    int? Duration,
    int? Price,
    ICollection<DanceId> DancesIds
);