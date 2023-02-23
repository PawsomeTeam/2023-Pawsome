using Microsoft.AspNetCore.Components;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages.StoreManagement.Products;

public class EditProductBase : ComponentBase
{
    [Parameter]
    public int Id { get; set; }
    
    [Inject]
    public IProductService ProductService { get; set; }
    
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    
    public ProductDto Product { get; set; }
    
    public string ErrorMessage { get; set; }
    
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
    protected async Task Update_Product_Click(int id, string name,string description, decimal price, int qty)
    {
        try
        {
            var productDto = new ProductDto
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    ImageURL = "/Images/Beauty/Beauty1.png",
                    Price = price,
                    Qty = qty,
                    CategoryId = 1,
                    CategoryName = "Beauty"
                };

                var returnedUpdateItemDto = await this.ProductService.UpdateItem(productDto);
                NavigationManager.NavigateTo("");
        }
        catch (Exception)
        {

            throw;
        }

    }
    
    protected async Task Delete_Product_Click(int id)
    {
        var cartItemDto = await ProductService.DeleteItem(id);
        NavigationManager.NavigateTo("");
    }

    
}