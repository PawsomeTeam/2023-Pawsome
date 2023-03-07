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

    public async Task<Order> AddItem(OrderDto orderDto)
    {
        Console.WriteLine("Repository AddItem.");
        List<OrderItem> OrderItems = new List<OrderItem>();
        var user = await this.pawsomeDbContext.Users.Where(u => u.Email == orderDto.UserEmail)
            .SingleOrDefaultAsync();
        Console.WriteLine("user ." + user.Email);
        // var order = await this.pawsomeDbContext.Orders.Where(o =>o.Id == orderDto.Id).SingleOrDefaultAsync();
        // if (order == null)
        // {
        //     Console.WriteLine("Order new? ");
        //     order = new Order
        //     {
        //         User = user,
        //         orderDate = DateTime.Now
        //     };
        //
        // }
        //  var item = await (from product in this.pawsomeDbContext.Products
        //     where product.Id == orderItemAddToDto.ProductId
        //     select new OrderItem
        //     {
        //         Order = order,
        //         OrderItemUser = user,
        //         OrderQuantity = orderItemAddToDto.Qty,
        //         Product = product,
        //         Price = product.Price
        //     }).SingleOrDefaultAsync();
        //
        // if (item != null)
        // {
        //     var result = await this.pawsomeDbContext.OrderItems.AddAsync(item);
        //     await this.pawsomeDbContext.SaveChangesAsync();
        //     return result.Entity;
        // }

        var item = new Order
        {
            User = user,
            orderDate = DateTime.Now,
            OrderItems = OrderItems
        };

        Console.WriteLine("Item , " + item.Id);
        foreach (var orderItem in orderDto.OrderItems)
        {
            var product = await this.pawsomeDbContext.Products.Where(p => p.Id == orderItem.ProductId)
                .SingleOrDefaultAsync();
            OrderItem newOrderItem = new OrderItem
            {
                OrderQuantity = orderItem.Qty,
                Product = product,
                Price = orderItem.Price
            };
            item.OrderItems.Add(newOrderItem);
        }

        Console.WriteLine("Item orders , " + item.OrderItems.Count);

        if (item != null)
        {
            await pawsomeDbContext.OrderItems.AddRangeAsync(item.OrderItems);
            var result = await pawsomeDbContext.Orders.AddAsync(item);
            await pawsomeDbContext.SaveChangesAsync();
            return result.Entity;
        }

        return null;
    }

    public async Task<Order> GetItem(string email)
    {
        var orders = await pawsomeDbContext.Orders.Include("OrderItems").Include("User")
            .Where(o => o.User.Email == email).ToListAsync();
        var order = orders.ElementAt(orders.Count - 1);
        order.OrderItems =
            await pawsomeDbContext.OrderItems.Include("Product").Where(o => o.Order.Id == order.Id).ToListAsync();
        Console.WriteLine("Repository  Id " + email + " order : " + order.Id);
        return order;
    }

    public async Task<IEnumerable<Order>> GetItems(string email)
    {
        var orders = await pawsomeDbContext.Orders.Include("OrderItems").Include("User")
            .Where(o => o.User.Email == email).ToListAsync();
        foreach (var order in orders)
        {
            order.OrderItems =
                await pawsomeDbContext.OrderItems.Include("Product").Where(o => o.Order.Id == order.Id).ToListAsync();
        }

        return orders;
    }

    public async Task<IEnumerable<Order>> AllItems()
    {
        var orders = await pawsomeDbContext.Orders.Include("OrderItems").Include("User").ToListAsync();
        foreach (var order in orders)
        {
            order.OrderItems =
                await pawsomeDbContext.OrderItems.Include("Product").Where(o => o.Order.Id == order.Id).ToListAsync();
        }
        return orders;
    }
}