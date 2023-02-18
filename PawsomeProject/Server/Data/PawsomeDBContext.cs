using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;


namespace PawsomeProject.Server.Data;

public class PawsomeDBContext : IdentityDbContext<User>
{
	public PawsomeDBContext(DbContextOptions<PawsomeDBContext> options) : base(options)
	{
	}
	

	public DbSet<Animal> Animals { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Cart> Carts { get; set; }
	public DbSet<CartItem> CartItems { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<ProductCategory> ProductCategories { get; set; }
	
}