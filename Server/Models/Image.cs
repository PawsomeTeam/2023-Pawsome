using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Models;

public class Image
{
    public int Id { get; set; }
    public string URL { get; set; }
    public string Type { get; set; }
    
    public Product? Product { get; set; }
    public Animal? Animal { get; set; }
}