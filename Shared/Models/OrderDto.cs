namespace PawsomeProject.Shared.Models;

public class OrderDto
{
    public int Id { get; set; }
    public string UserEmail { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public DateTime purchasedDate { get; set; }
}