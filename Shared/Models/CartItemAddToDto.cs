namespace PawsomeProject.Shared.Models;

public class CartItemAddToDto
{
    public string UserEmail { get; set; }
    public int ProductId { get; set; }
    public int Qty { get; set; }
}