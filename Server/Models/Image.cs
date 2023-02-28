using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PawsomeProject.Server.Models;

public class Image
{
    public int Id { get; set; }
    public string URL { get; set; }
    public string Type { get; set; }
    
    public Product Product { get; set; }
}