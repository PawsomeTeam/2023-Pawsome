using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IOrderService
{
    Task<OrderDto> GetItem(string email);
    Task<List<OrderDto>> GetItems(string email);
    Task<List<OrderDto>> AllItems();
    Task<OrderDto> AddItem(OrderDto orderDto);
}