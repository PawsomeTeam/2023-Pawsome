using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IOrderService
{
    Task<OrderDto> GetItem(int id);
    Task<OrderDto> AddItem(OrderDto orderDto);
}