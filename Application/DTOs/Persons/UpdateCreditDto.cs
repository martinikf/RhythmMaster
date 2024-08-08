using Domain.Entities;

namespace Application.DTOs.Persons;

public record UpdateCreditDto
(
    PersonId UserId, 
    int CreditDifference
);