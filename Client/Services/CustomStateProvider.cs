using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public class CustomStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService api;
    private CurrentUser currentUser;

    public CustomStateProvider(IAuthService api)
    {
        this.api = api;
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        try
        {
            var userInfo = await GetCurrentUser();
            if (userInfo.IsAuthenticated)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, currentUser.UserName)
                }.Concat(currentUser.Claims
                    .Select(c => new Claim(c.Key, c.Value)));
                identity = new ClaimsIdentity(claims, "Server authentication");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Request failed:" + ex.ToString());
        }

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
    
    private async Task<CurrentUser> GetCurrentUser()
    {
        if (currentUser != null && currentUser.IsAuthenticated) return currentUser;
        currentUser = await api.CurrentUserInfo();
        return currentUser;
    }
    
    public async Task Logout()
    {
        await api.Logout();
        currentUser = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    
    public async Task Login(LoginRequest loginParameters)
    {
        await api.Login(loginParameters);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    
    public async Task Register(RegisterRequest registerParameters)
    {
        await api.Register(registerParameters);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Update(string email, UpdateRequest updateRequest)
    {
        await api.UpdateUser(email, updateRequest);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}