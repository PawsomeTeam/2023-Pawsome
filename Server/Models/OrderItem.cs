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

//
// var item = new Order
// {
//     User = user,
//     orderDate = DateTime.Now
// };
//         
// foreach (var orderItem in orderDto.OrderItems)
// {
//     var product = await this.pawsomeDbContext.Products.Where(p =>p.Id == orderItem.ProductId).SingleOrDefaultAsync();
//     OrderItem newOrderItem = new OrderItem
//     {
//         OrderQuantity = orderItem.Qty,
//         Product = product,
//         Price = orderItem.Price
//     };
//     item.OrderItems.Add(newOrderItem);
//
// }