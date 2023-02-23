using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages;

public class CheckoutBase : ComponentBase
{
    [Inject]
    public IJSRuntime Js { get; set; }
    
    protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
    protected int TotalQty { get; set; }
    protected string PaymentDescription { get; set; }
    
    protected decimal PaymentAmount { get; set; }
    
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ShoppingCartService.GetItems(1);
            if (ShoppingCartItems != null)
            {
                Guid orderGuid = Guid.NewGuid();

                PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                PaymentDescription = $"O_{1}_{orderGuid}";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await Js.InvokeVoidAsync("initPayPalButton");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}