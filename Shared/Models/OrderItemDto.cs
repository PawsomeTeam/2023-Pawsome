namespace PawsomeProject.Shared.Models;

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string  ProductImageURL { get; set; }
    public decimal Price { get; set; }
    public int Qty { get; set; }
    public DateTime purchasedDate { get; set; }
}