using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Persons;
using Domain.Common;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IUserService
{
    public Task<Result<string>> Login(LoginCredentialsDto data);

    public Task<Result> Register(RegisterUserDto data);
    
    public Task<Result> CreateInterpret(RegisterInterpretDto data);
    
    public Task<Result> ChangeRole(ChangeRoleDto data);

    public Task<Result<AppUser>> GetUserByIdAsync(PersonId id);

    public Task<Result<AppUser>> GetUserByEmail(string email);
}