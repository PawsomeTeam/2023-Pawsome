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

    public ProductDto? Product { get; set; }

    public string? ErrorMessage { get; set; } = null;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Product = await ProductService.GetItem(Id);
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
            var cartItemDto = await ShoppingCartService.AddItem(cartItemAddToDto);
            NavigationManager.NavigateTo("/ShoppingCart");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected async Task DeleteProductItem_Click(int id)
    {
        await ProductService.DeleteItem(id);
        NavigationManager.NavigateTo("");
    }

}