using Domain.Entities;

namespace Application.DTOs.Persons;

public record PurchaseSongDto
(
    PersonId UserId, 
    SongId SongId
);