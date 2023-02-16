using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IAuthService
{
    Task Login(LoginRequest loginRequest);
    Task Register(RegisterRequest registerRequest);
    Task Logout();
    Task<CurrentUser> CurrentUserInfo();
}