using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories;

public interface IOrderRepository
{
    Task<Order> AddItem(OrderDto orderDto);
    Task<Order> GetItem(string email);
    Task<IEnumerable<Order>> GetItems(string email);
    Task<IEnumerable<Order>> AllItems();
}