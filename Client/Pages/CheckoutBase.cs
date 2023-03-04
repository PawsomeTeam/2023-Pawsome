using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages;

public class CheckoutBase : ComponentBase
{
    [Inject]
    public IJSRuntime Js { get; set; }

    protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; } = new List<CartItemDto>();

    protected OrderItemDto OrderItems { get; set; }
    protected int TotalQty { get; set; }
    protected string PaymentDescription { get; set; }
    
    protected static int OrderId { get; set; }
    protected decimal PaymentAmount { get; set; }
    [Inject] 
    public IAuthService authService { get; set; } = default!;
    private CurrentUser CurrentUser { get; set; } = new CurrentUser();
    
    [Inject]
    public IOrderService OrderService { get; set; }
    
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }



    protected override async Task OnInitializedAsync()
    {
        try
        {
            CurrentUser = await authService.CurrentUserInfo();
            ShoppingCartItems = await ShoppingCartService.GetItems(CurrentUser.Email);

            if (ShoppingCartItems != null)
            {
                Guid orderGuid = Guid.NewGuid();
                PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                PaymentDescription = $"O_{1}_{orderGuid}";
                OrderId = ++OrderId;
                AddToOrder(ShoppingCartItems);
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
    protected async Task AddToOrder(IEnumerable<CartItemDto> ShoppingCartItems)
    {
        try
        {
            List<OrderItemDto> orderItemDtos = new List<OrderItemDto>();
            foreach (var cart in ShoppingCartItems)
            {
                var orderItemDto = new OrderItemDto()
                {
                   
                    ProductId = cart.ProductId,
                    ProductName = cart.ProductName,
                    ProductImageURL = cart.ProductImageURL,
                    Price = cart.Price,
                    Qty = cart.Qty

                };
                orderItemDtos.Add(orderItemDto);
            }

            var OrderDto = new OrderDto
            {
                UserEmail = CurrentUser.Email,
                OrderItems = orderItemDtos,
                purchasedDate = DateTime.Now
            };
            
            await this.OrderService.AddItem(OrderDto);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}