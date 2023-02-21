using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IAuthService
{
    Task Login(LoginRequest loginRequest);
    Task Logout();
    Task Register(RegisterRequest registerRequest);
    Task UpdateUser(string email, UpdateRequest updateRequest);
    Task DeleteUser(string email);
    Task<CurrentUser> CurrentUserInfo();
    Task<List<FindUser>> GetAllUsers();
    Task<FindUser> GetUserByEmail(string email);
}