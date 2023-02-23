using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IShoppingCartService
{
    Task<List<CartItemDto>> GetItems(int userId);
    Task<CartItemDto> AddItem(CartItemAddToDto cartItemAddToDto);
    Task<CartItemDto> DeleteItem(int id);
    Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);

    event Action<int> OnShoppingCartChanged;
    void RaiseEventOnShoppingCartChanged(int totalQty);
} 