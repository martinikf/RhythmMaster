using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public abstract class Person : Entity<PersonId>
{
    [Required, MaxLength(64)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required, MaxLength(64)]
    public string LastName { get; set; } = string.Empty;
    
    [MaxLength(128)]
    public string? CustomName { get; set; }
    
    [MaxLength(1024)]
    public string? SocialLink { get; set; } 

    
    protected Person(){}
    
    protected Person(string firstName, string lastName, string? customName, string? socialLink)
    {
        FirstName = firstName;
        LastName = lastName;
        CustomName = customName;
        SocialLink = socialLink;
    }
    
    protected Person(PersonId id, string firstName, string lastName, string? customName, string? socialLink) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        CustomName = customName;
        SocialLink = socialLink;
    }
}

public record PersonId(int Value);