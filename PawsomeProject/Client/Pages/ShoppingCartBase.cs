using Microsoft.AspNetCore.Components;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages;

public class ShoppingCartBase:ComponentBase
{
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }

    public IEnumerable<CartItemDto> ShoppingCartItems { get; set; }

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
}