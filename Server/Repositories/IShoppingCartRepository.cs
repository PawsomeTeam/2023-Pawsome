using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories;

public interface IShoppingCartRepository
{
    Task<CartItem> AddItem(CartItemAddToDto cartItemAddToDto);
    Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
    Task<CartItem> DeleteItem(int id);
    Task<CartItem> GetItem(int id);
    Task<IEnumerable<CartItem>> GetItems(int userId);

}