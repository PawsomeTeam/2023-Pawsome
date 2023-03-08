using Microsoft.AspNetCore.Components;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages.StoreManagement.Products;

public class ProductDetailsBase : ComponentBase
{
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public IProductService ProductService { get; set; } = default!;

    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject] public IAuthService authService { get; set; } = default!;
    public ProductDto? Product { get; set; }

    public CurrentUser CurrentUser { get; set; } = new CurrentUser();

    public string? ErrorMessage { get; set; } = null;

    public string mainImgUrl { get; set; } = "";


    protected override async Task OnInitializedAsync()
    {
        try
        {
            Product = await ProductService.GetItem(Id);
            mainImgUrl = Product.Images[0].URL;
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }

    protected async Task AddToCart_Click(CartItemAddToDto cartItemAddToDto)
    {

        try
        {
            CurrentUser = await authService.CurrentUserInfo();
            cartItemAddToDto.UserEmail = CurrentUser.Email;
            var cartItemDto = await ShoppingCartService.AddItem(cartItemAddToDto);
            NavigationManager.NavigateTo("/ShoppingCart");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}