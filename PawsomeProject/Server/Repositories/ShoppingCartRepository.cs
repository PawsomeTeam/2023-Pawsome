using Microsoft.EntityFrameworkCore;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly PawsomeDBContext pawsomeDbContext;

    public ShoppingCartRepository(PawsomeDBContext pawsomeDbContext)
    {
        this.pawsomeDbContext = pawsomeDbContext;
    }

    private async Task<bool> CartItemExists(int cartId, int productId)
    {
        return await this.pawsomeDbContext.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
    }

    public async Task<CartItem> AddItem(CartItemAddToDto cartItemAddToDto)
    {
        if (await CartItemExists(cartItemAddToDto.CartId, cartItemAddToDto.ProductId) == false)
        {
            var item = await (from product in this.pawsomeDbContext.Products
                where product.Id == cartItemAddToDto.ProductId
                select new CartItem
                {
                    CartId = cartItemAddToDto.CartId,
                    ProductId = product.Id,
                    Qty = cartItemAddToDto.Qty
                }).SingleOrDefaultAsync();
            if (item != null)
            {
                var result = await this.pawsomeDbContext.CartItems.AddAsync(item);
                await this.pawsomeDbContext.SaveChangesAsync();
                return result.Entity;
            }
        }


        return null;
    }

    public Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        throw new NotImplementedException();
    }

    public Task<CartItem> DeleteItem(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CartItem> GetItem(int id)
    {
        return await (from cart in this.pawsomeDbContext.Carts
            join cartItem in this.pawsomeDbContext.CartItems
                on cart.Id equals cartItem.CartId
            where cartItem.Id == id
            select new CartItem
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                Qty = cartItem.Qty,
                CartId = cartItem.CartId
            }).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<CartItem>> GetItems(int userId)
    {
        return await (from cart in this.pawsomeDbContext.Carts
            join cartItem in this.pawsomeDbContext.CartItems
                on cart.Id equals cartItem.CartId
            where cart.UserId == userId
            select new CartItem
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                Qty = cartItem.Qty,
                CartId = cartItem.CartId
            }).ToListAsync();
    }
}