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

    public async Task UpdateUser(string email, UpdateRequest updateRequest)
    {
        var result = await httpClient.PutAsJsonAsync($"api/User/UpdateUser/{email}", updateRequest);
        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
        result.EnsureSuccessStatusCode();
    }

    public async Task DeleteUser(string email)
    {
        var result = await httpClient.DeleteAsync($"api/User/GetUserByEmail/{email}");
        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
        result.EnsureSuccessStatusCode();
    }
    public async Task<CurrentUser> CurrentUserInfo()
    {
        var result = await httpClient.GetFromJsonAsync<CurrentUser>("api/User/CurrentUserInfo");
        return result;
    }

    public async Task<List<FindUser>> GetAllUsers()
    {
        var result = await httpClient.GetFromJsonAsync<List<FindUser>>("api/User/GetAllUsers");
        return result;
    }

    public async Task<FindUser> GetUserByEmail(string email)
    {
        var result = await httpClient.GetFromJsonAsync<FindUser>($"api/User/GetUserByEmail/{email}");
        return result;
    }
}