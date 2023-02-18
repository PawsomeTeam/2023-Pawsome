using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IShoppingCartService
{
    Task<IEnumerable<CartItemDto>> GetItems(int userId);
    Task<CartItemDto> AddItems(CartItemAddToDto cartItemAddToDto);
}