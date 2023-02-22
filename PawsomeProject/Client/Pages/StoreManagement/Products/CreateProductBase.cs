using Microsoft.AspNetCore.Components;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages.StoreManagement.Products;

public class CreateProductBase : ComponentBase
{
    [Parameter]
    public int Id { get; set; }
    
    [Inject]
    public IProductService ProductService { get; set; }
    
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public ProductDto Product { get; set; } = new ProductDto();
    
    public string ErrorMessage { get; set; }

  
    protected async Task Create_Product_Click(string name,string description, decimal price, int qty)
    {
        try
        {
            var productDto = new ProductDto
                {
                    Id = Id,
                    Name = name,
                    Description = description,
                    ImageURL = "/Images/Beauty/Beauty1.png",
                    Price = price,
                    Qty = qty,
                    CategoryId = 1,
                    CategoryName = "Beauty"
                };

               await this.ProductService.CreateItem(productDto);
               NavigationManager.NavigateTo("");
        }
        catch (Exception)
        {

            throw;
        }

    }
    
    
}