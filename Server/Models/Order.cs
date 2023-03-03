using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Models;

public class Order
{
    public int Id { get; set; }
    public User User { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public DateTime orderDate { get; set; }
  
}