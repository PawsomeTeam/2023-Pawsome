using System.Net.Http.Json;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient httpClient;

    public AuthService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async Task<CurrentUser> CurrentUserInfo()
    {
        var result = await httpClient.GetFromJsonAsync<CurrentUser>("api/User/CurrentUserInfo");
        return result;
    }
    
    public async Task Login(LoginRequest loginRequest)
    {
        var result = await httpClient.PostAsJsonAsync("api/User/login", loginRequest);
        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
        result.EnsureSuccessStatusCode();
    }
    
    public async Task Logout()
    {
        var result = await httpClient.PostAsync("api/User/logout", null);
        result.EnsureSuccessStatusCode();
    }

    public async Task Register(RegisterRequest registerRequest)
    {
        var result = await httpClient.PostAsJsonAsync("api/User/register", registerRequest);
        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
        result.EnsureSuccessStatusCode();
    }
}