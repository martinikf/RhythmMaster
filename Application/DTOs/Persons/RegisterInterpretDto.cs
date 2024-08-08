namespace Application.DTOs.Persons;

public record RegisterInterpretDto
(
    string FirstName,
    string LastName,
    string? CustomName,
    string? SocialLink
);