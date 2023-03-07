namespace PawsomeProject.Shared.Models;

public class OrderItemAddToDto
{
    public string UserEmail { get; set; }
    
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Qty { get; set; }
}