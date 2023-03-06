using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }
    
    public Order? Order { get; set; }
    
    public int OrderQuantity { get; set; }

    public Product? Product { get; set; }
    
    public decimal Price { get; set; }
}
