namespace PawsomeProject.Shared.Models;

public class CartItemAddToDto
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Qty { get; set; }
}