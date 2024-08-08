namespace Application.Interfaces.Providers;

public interface IPasswordHashProvider
{
    string Hash(string password, string username);
}