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

    private async Task<bool> CartItemExists(string userEmail, int productId)
    {
        return await this.pawsomeDbContext.CartItems.AnyAsync(c =>
            c.Cart.User.Email == userEmail && c.Product.Id == productId);
    }

    public async Task<CartItem> AddItem(CartItemAddToDto cartItemAddToDto)
    {
        // find user
        // find cart with user.id
        if (await CartItemExists(cartItemAddToDto.UserEmail, cartItemAddToDto.ProductId) == false)
        {
            var user = await this.pawsomeDbContext.Users.Where(u => u.Email == cartItemAddToDto.UserEmail)
                .SingleOrDefaultAsync();
            var cart = await this.pawsomeDbContext.Carts.Where(c => c.User.Id == user.Id).SingleOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart
                {
                    User = user
                };
            }

            var item = await (from product in this.pawsomeDbContext.Products
                where product.Id == cartItemAddToDto.ProductId
                select new CartItem
                {
                    Cart = cart,
                    Product = product,
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

    public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        var item = await this.pawsomeDbContext.CartItems.Include("Product").Where(o => o.Id == id).FirstAsync();
        if (item != null)
        {
            item.Qty = cartItemQtyUpdateDto.Qty;
            await this.pawsomeDbContext.SaveChangesAsync();
            return item;
        }

        return null;
    }

    public async Task<CartItem> DeleteItem(int id)
    {
        var item = await this.pawsomeDbContext.CartItems.FindAsync(id);
        if (item != null)
        {
            this.pawsomeDbContext.CartItems.Remove(item);
            await this.pawsomeDbContext.SaveChangesAsync();
        }

        return item;
    }

    public async Task<CartItem> GetItem(int id)
    {
        var cartItem = await pawsomeDbContext.CartItems.Where(c => c.Id == id).SingleOrDefaultAsync();
        return cartItem;
    }

    public async Task<IEnumerable<CartItem>> GetItems(string userEmail)
    {
        // var cart = await pawsomeDbContext.Carts.Include("UserId").Where(c => c.UserId.Id == userId).SingleOrDefaultAsync();
        var cartItems = await pawsomeDbContext.CartItems.Where(c => c.Cart.User.Email == userEmail).ToListAsync();
        return cartItems;
    }
}