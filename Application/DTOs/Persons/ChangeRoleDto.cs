using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.Persons;

public record ChangeRoleDto
(
    PersonId UserId, 
    Role Role
);