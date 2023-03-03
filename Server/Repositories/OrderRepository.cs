using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly PawsomeDBContext pawsomeDbContext;

    public OrderRepository(PawsomeDBContext pawsomeDbContext)
    {
        this.pawsomeDbContext = pawsomeDbContext;
    }

    public async Task<OrderItem> AddItem(OrderItemAddToDto orderItemAddToDto)
    {
        
        var user = await this.pawsomeDbContext.Users.Where(u => u.Email == orderItemAddToDto.UserEmail)
            .SingleOrDefaultAsync();
        Console.WriteLine("Order Item ID ? " + orderItemAddToDto.OrderId);
        var order = await this.pawsomeDbContext.Orders.Where(o =>o.Id == orderItemAddToDto.OrderId).SingleOrDefaultAsync();
         if (order == null)
        {
            Console.WriteLine("Order new? ");
            order = new Order
            {
                Id = orderItemAddToDto.OrderId,
                User = user,
                orderDate = DateTime.Now
            };

        }
         var item = await (from product in this.pawsomeDbContext.Products
            where product.Id == orderItemAddToDto.ProductId
            select new OrderItem
            {
                Order = order,
                OrderItemUser = user,
                OrderQuantity = orderItemAddToDto.Qty,
                Product = product,
                Price = product.Price
            }).SingleOrDefaultAsync();
       
        if (item != null)
        {
            var result = await this.pawsomeDbContext.OrderItems.AddAsync(item);
            await this.pawsomeDbContext.SaveChangesAsync();
            return result.Entity;
        }

        return null;
    }

    public async Task<OrderItem> GetItem(int id)
    {
        var orderItem = await pawsomeDbContext.OrderItems.Where(o => o.OrderItemId == id).SingleOrDefaultAsync();
        return orderItem;
    }
}