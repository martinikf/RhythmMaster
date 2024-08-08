namespace Application.DTOs.Persons;

public record RegisterUserDto
(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? CustomName,
    string? SocialLink
);