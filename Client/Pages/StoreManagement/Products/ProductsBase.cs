using Microsoft.AspNetCore.Components;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages.StoreManagement.Products;

public class ProductsBase : ComponentBase
{
    [Inject] public IProductService ProductService { get; set; } = default!;

    [Inject] public IShoppingCartService ShoppingCartService { get; set; } = default!;

    [Inject] public IAuthService authService { get; set; } = default!;
    public IEnumerable<ProductDto>? Products { get; set; }

    private CurrentUser CurrentUser { get; set; } = new CurrentUser();
    public bool IsLoading = true;

    protected override async Task OnInitializedAsync()
    {
        Products = await ProductService.GetItems();
        CurrentUser = await authService.CurrentUserInfo();
        var shoppingCartItems = await ShoppingCartService.GetItems(CurrentUser.Email);
        IsLoading = false;
        var totalQty = shoppingCartItems.Sum(i => i.Qty);
        ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
    }

    public string? ErrorMessage { get; set; } = null;

    protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
    {
        return from product in Products
            group product by product.CategoryId
            into prodByCatGroup
            orderby prodByCatGroup.Key
            select prodByCatGroup;
    }

    protected string? GetCategoryName(IGrouping<int, ProductDto> groupedProductDtos)
    {
        return groupedProductDtos.FirstOrDefault(pg => pg.CategoryId == groupedProductDtos.Key)?.CategoryName;
    }
}