using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages;

public class ShoppingCartBase : ComponentBase
{
    [Inject] public IShoppingCartService ShoppingCartService { get; set; }

    public List<CartItemDto> ShoppingCartItems { get; set; } = new List<CartItemDto>();
    
    [Inject]
    public IJSRuntime Js { get; set; }
    public string ErrorMessage { get; set; }
    protected string TotalPrice { get; set; }
    protected int TotalQuantity { get; set; }
    [Inject] 
    public IAuthService authService { get; set; } = default!;
    private CurrentUser CurrentUser { get; set; } = new CurrentUser();
    protected override async Task OnInitializedAsync()
    {
        try
        {
            CurrentUser = await authService.CurrentUserInfo();
            ShoppingCartItems = await ShoppingCartService.GetItems(CurrentUser.Email);
            CartChange();
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
        CartChange();
    }
    
    protected async Task DeleteAllCart()
    {
        var cartItemDto = await ShoppingCartService.DeleteAllItems(CurrentUser.Email);
        ShoppingCartItems.Remove(cartItemDto);
        CartChange();
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

    private void SetTotalPrice()
    {
        TotalPrice = this.ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
    }

    private void SetTotalQuantity()
    {
        TotalQuantity = this.ShoppingCartItems.Sum(p => p.Qty);

    }

    private void CalculateCartSummaryTotals()
    {
        SetTotalPrice();
        SetTotalQuantity();
    }

    private void UpdateItemTotalPrice(CartItemDto cartItemDto)
    {
        var item = GetCartItem(cartItemDto.Id);
        if (item != null)
        {
            item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
        }
    }

    protected async Task UpdateQty_Input(int id)
    {
        await MakeUpdateQtyButtonVisible(id, true);
    }

    private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
    {
        await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
    }
    
    protected async Task UpdateQtyCartItem_Click(int id, int qty)
    {
        try
        {
            if (qty > 0)
            {
                var updateItemDto = new CartItemQtyUpdateDto
                {
                    CartItemId = id,
                    Qty = qty
                };

                var returnedUpdateItemDto = await this.ShoppingCartService.UpdateQty(updateItemDto);

                UpdateItemTotalPrice(returnedUpdateItemDto);
                CartChange();
                await MakeUpdateQtyButtonVisible(id, false);

            }
            else
            {
                var item = this.ShoppingCartItems.FirstOrDefault(i => i.Id == id);

                if (item != null)
                {
                    item.Qty = 1;
                    item.TotalPrice = item.Price;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    private void CartChange()
    {
        CalculateCartSummaryTotals();
        ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
    }
    
}