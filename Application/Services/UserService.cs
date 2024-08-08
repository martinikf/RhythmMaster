using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Persons;
using Application.Interfaces.IRepositories;
using Application.Interfaces.Providers;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Entities;

namespace Application.Services;

public class UserService(IAppUserRepository appUserRepository, 
    IInterpretRepository interpretRepository, 
    IJwtProvider jwtProvider, 
    IPasswordHashProvider hasher
    ) : IUserService
{
    private readonly IAppUserRepository _appUserRepository = appUserRepository;
    private readonly IInterpretRepository _interpretRepository = interpretRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IPasswordHashProvider _hasher = hasher;
    
    private const int NewUserDefaultCredit = 0;
    
    public async Task<Result<string>> Login(LoginCredentialsDto data)
    {
        var email = data.Email.Trim().ToLower();

        var dj = await _appUserRepository.GetAppUserByEmailAsync(email);

        if (dj is null)
            return new NotFoundError<string>("User with this email doesn't exist.");
        
        var passwordHash = _hasher.Hash(data.Password, email);
        
        if (dj.Password != passwordHash)
            return new ErrorResult<string>("Password is incorrect.");
        
        var token = _jwtProvider.Generate(dj);

        return new SuccessResult<string>(token);
    }

    public async Task<Result> Register(RegisterUserDto data)
    {
        var email = data.Email.Trim().ToLower();

        var dj = await  _appUserRepository.GetAppUserByEmailAsync(email);
        
        if(dj != null)
        {
            return new ErrorResult("Email address has been already used.");

        }

        var passwordHash = _hasher.Hash(data.Password, email);

        dj = new AppUser()
        {
            Email = email,
            Password = passwordHash,
            FirstName = data.FirstName,
            LastName = data.LastName,
            CustomName = data.CustomName,
            SocialLink = data.SocialLink,
            CreditBalance = NewUserDefaultCredit
        };
        
        await _appUserRepository.Add(dj);

        return new SuccessResult();
    }

    public async Task<Result> CreateInterpret(RegisterInterpretDto data)
    {
        var interpret = new Interpret()
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            CustomName = data.CustomName,
            SocialLink = data.SocialLink
        };
        
        await _interpretRepository.Add(interpret);
        
        return new SuccessResult();
    }

    public async Task<Result> ChangeRole(ChangeRoleDto data)
    {
        var user = await _appUserRepository.GetAppUserByIdAsync(data.UserId);
        
        if (user == null)
        {
            return new NotFoundError($"User with id={data.UserId} not found.");
        }

        user.Role = data.Role;
        await _appUserRepository.Update(user);

        return new SuccessResult();
    }

    public async Task<Result<AppUser>> GetUserByIdAsync(PersonId id)
    {
        var user = await _appUserRepository.GetAppUserByIdAsync(id);
        
        if (user == null)
        {
            return new NotFoundError<AppUser>($"User with id={id} not found.");
        }

        return new SuccessResult<AppUser>(user);
    }

    public async Task<Result<AppUser>> GetUserByEmail(string email)
    {
        var user = await _appUserRepository.GetAppUserByEmailAsync(email);
        
        if (user == null)
        {
            return new NotFoundError<AppUser>($"User with email={email} not found.");
        }

        return new SuccessResult<AppUser>(user);
    }
}