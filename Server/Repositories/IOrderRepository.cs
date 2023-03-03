using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories;

public interface IOrderRepository
{
    Task<OrderItem> AddItem(OrderItemAddToDto orderItemAddToDto);
    Task<OrderItem> GetItem(int id);
}