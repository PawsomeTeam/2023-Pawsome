using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IOrderService
{
    Task<OrderDto> GetItem(string email);
    Task<OrderDto> AddItem(OrderDto orderDto);
}