using Microsoft.AspNetCore.Identity;

namespace PawsomeProject.Shared.Models;

public class CurrentUser
{
    public bool IsAuthenticated { get; set; }
    
    public string UserName { get; set; }
    
    public string FullName { get; set; }
    
    public string Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public Dictionary<string, string> Claims { get; set; }
}