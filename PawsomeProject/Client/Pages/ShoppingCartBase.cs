using Microsoft.AspNetCore.Components;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages;

public class ShoppingCartBase : ComponentBase
{
    [Inject] public IShoppingCartService ShoppingCartService { get; set; }

    public List <CartItemDto> ShoppingCartItems { get; set; }

    public string ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ShoppingCartService.GetItems(1);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected async Task DeleteCartItem_Click(int id)
    {
        var cartItemDto = await ShoppingCartService.DeleteItem(id);
        RemoveCartItem(id);
    }

    private CartItemDto GetCartItem(int id)
    {
        return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
    }

    private void RemoveCartItem(int id)
    {
        var cartItemDto = GetCartItem(id);
        ShoppingCartItems.Remove(cartItemDto);
    }
}