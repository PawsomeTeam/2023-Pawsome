using Microsoft.AspNetCore.Components;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages.StoreManagement.Products;

public class ProductsBase : ComponentBase
{
    [Inject]
    public IProductService ProductService { get; set; } = default!;

    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; } = default!;

    public IEnumerable<ProductDto>? Products { get; set; }
    // public IEnumerable<ProductCategoryDto> Categories { get; set; }

    public bool IsLoading = true;

    protected override async Task OnInitializedAsync()
    {
        Products = await ProductService.GetItems();
        // Categories = await ProductService.GetCategories();
        var shoppingCartItems = await ShoppingCartService.GetItems(1);
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