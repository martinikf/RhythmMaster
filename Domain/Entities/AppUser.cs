using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class AppUser : Person
{
    [Required, MaxLength(256)] 
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    public int CreditBalance { get; set; }
    
    public Role Role { get; set; }
    
    private ICollection<UserSong>? _songs;

    public ICollection<UserSong> BoughtSongs
    {
        get { return _songs ??= new HashSet<UserSong>(); }
    }
    
    private ICollection<Playlist>? _playlists;

    public ICollection<Playlist> Playlists
    {
        get { return _playlists ??= new HashSet<Playlist>(); }
    }

    public AppUser(){}
    
    public AppUser(string email, string password, Role role, string firstName, string lastName, string? customName, string? socialLink) : base(firstName, lastName, customName, socialLink)
    {
        Email = email;
        Password = password;
        Role = role;
    }
    
    public AppUser(PersonId id, string email, string password, Role role, string firstName, string lastName, string? customName, string? socialLink) : base(id, firstName, lastName, customName, socialLink)
    {
        Email = email;
        Password = password;
        Role = role;
    }
}