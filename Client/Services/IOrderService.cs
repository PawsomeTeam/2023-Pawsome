using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IOrderService
{
    Task<OrderItemDto> GetItem(int id);
    Task<OrderItemDto> AddItem(OrderItemAddToDto orderItemAddToDto);
}