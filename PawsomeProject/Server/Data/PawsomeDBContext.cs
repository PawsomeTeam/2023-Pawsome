using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Data;

public class PawsomeDBContext : IdentityDbContext<User>
{
    public PawsomeDBContext(DbContextOptions<PawsomeDBContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
}