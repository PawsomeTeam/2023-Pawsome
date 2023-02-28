using System.ComponentModel.DataAnnotations;

namespace PawsomeProject.Shared.Models;

public class UpdateRequest
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    public List<string> Roles { get; set; }
    
    public string? PhoneNumber { get; set; }
}