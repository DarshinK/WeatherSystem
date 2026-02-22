using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using BCrypt.Net;

namespace AuthService.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IJwtProvider _jwtProvider;

    public AuthService(IUserRepository repository, IJwtProvider jwtProvider)
    {
        _repository = repository;
        _jwtProvider = jwtProvider;
    }

    public async Task RegisterAsync(string email, string password)
    {
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Email = email,
            PasswordHash = hashed
        };

        await _repository.AddAsync(user);
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _repository.GetByEmailAsync(email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        return _jwtProvider.GenerateToken(user.Email);
    }
}
